terraform {
  required_providers {
    aws = {
      source = "hashicorp/aws"
      version = "~> 5.40.0"
    }
  }

  required_version = ">= 1.7.4"

  backend "s3" {
    bucket = "code-learning-beanstalk-bucket"
    key = "infrastructure/api/state-files"
    region = "eu-west-1"
  }
}

provider "aws" {
  region = "eu-west-1"
}

resource "aws_vpc" "code_learning_api_vpc" {
  cidr_block = "10.0.0.0/16"
  enable_dns_support = true
  enable_dns_hostnames = true
  tags = {
    Name = "code_learning_api_vpc"
  }
}

resource "aws_internet_gateway" "code_learning_api_gateway" {
  vpc_id = aws_vpc.code_learning_api_vpc.id
  tags = {
    owner: "liam.talberg@bbd.co.za"
  }
}

resource "aws_subnet" "code_learning_api_subnet_a" {
  vpc_id                  = aws_vpc.code_learning_api_vpc.id
  cidr_block              = "10.0.4.0/24"
  map_public_ip_on_launch = true
  availability_zone       = "eu-west-1a"
  tags = {
    owner: "liam.talberg@bbd.co.za"
  }
}

resource "aws_route_table" "code_learning_api_route_table_a" {
  vpc_id = aws_vpc.code_learning_api_vpc.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.code_learning_api_gateway.id
  }

  tags = {
    owner: "liam.talberg@bbd.co.za"
  }
}

resource "aws_route_table_association" "code_learning_api_association_a" {
  subnet_id      = aws_subnet.code_learning_api_subnet_a.id
  route_table_id = aws_route_table.code_learning_api_route_table_a.id
}

resource "aws_subnet" "code_learning_api_subnet_b" {
  vpc_id                  = aws_vpc.code_learning_api_vpc.id
  cidr_block              = "10.0.5.0/24"
  map_public_ip_on_launch = true
  availability_zone       = "eu-west-1b"
  tags = {
    owner: "liam.talberg@bbd.co.za"
  }
}

resource "aws_route_table" "code_learning_api_route_table_b" {
  vpc_id = aws_vpc.code_learning_api_vpc.id

  route {
    cidr_block = "0.0.0.0/0"
    gateway_id = aws_internet_gateway.code_learning_api_gateway.id
  }

  tags = {
    owner: "liam.talberg@bbd.co.za"
  }
}

resource "aws_route_table_association" "code_learning_api_association_b" {
  subnet_id      = aws_subnet.code_learning_api_subnet_b.id
  route_table_id = aws_route_table.code_learning_api_route_table_b.id
}

resource "aws_db_subnet_group" "code_learning_api_subnet_group" {
  name       = "code_learning_api_subnet_group"
  subnet_ids = [aws_subnet.code_learning_api_subnet_a.id, aws_subnet.code_learning_api_subnet_b.id]

  tags = {
    owner: "liam.talberg@bbd.co.za"
  }
}

resource "aws_iam_role" "beanstalk_ec2" {
  assume_role_policy    = "{\"Statement\":[{\"Action\":\"sts:AssumeRole\",\"Effect\":\"Allow\",\"Principal\":{\"Service\":\"ec2.amazonaws.com\"}}],\"Version\":\"2012-10-17\"}"
  description           = "Allows EC2 instances to call AWS services on your behalf."
  force_detach_policies = false
  managed_policy_arns   = ["arn:aws:iam::aws:policy/AWSElasticBeanstalkMulticontainerDocker", "arn:aws:iam::aws:policy/AWSElasticBeanstalkWebTier", "arn:aws:iam::aws:policy/AWSElasticBeanstalkWorkerTier"]
  max_session_duration  = 3600
  name                  = "aws-elasticbeanstalk-ec2"
  path                  = "/"
}

resource "aws_iam_instance_profile" "beanstalk_ec2" {
  name = "aws-code_learning-ec2-profile"
  role = aws_iam_role.beanstalk_ec2.name
}

resource "aws_security_group" "code_learning_api_security_group" {
  vpc_id = aws_vpc.code_learning_api_vpc.id

  tags = {
    owner: "liam.talberg@bbd.co.za"
  }
}

resource "aws_vpc_security_group_egress_rule" "allow_all_traffic_ipv4" {
  security_group_id = aws_security_group.code_learning_api_security_group.id
  cidr_ipv4         = "0.0.0.0/0"
  ip_protocol       = "-1" # semantically equivalent to all ports
}

resource "aws_vpc_security_group_ingress_rule" "allow_tls_ipv4" {
  security_group_id = aws_security_group.code_learning_api_security_group.id
  cidr_ipv4         = aws_vpc.code_learning_api_vpc.cidr_block
  from_port         = 443
  ip_protocol       = "tcp"
  to_port           = 443
}

# S3 bucket to store jar
resource "aws_s3_bucket" "beanstalk_bucket" {
  bucket = "code-learning-beanstalk-bucket"
}

# Add .exe to bucket
resource "aws_s3_object" "app_exe" {
  bucket = aws_s3_bucket.beanstalk_bucket.id
  key    = "CodeLearningSpectaclesAPI.exe"
  source = "release/CodeLearningSpectaclesAPI.exe"
  etag = filemd5("release/CodeLearningSpectaclesAPI.exe")
  depends_on = [aws_s3_bucket.beanstalk_bucket]
}

# create app
resource "aws_elastic_beanstalk_application" "app" {
  name        = "code-learning-api"
  description = "code learning API"
}

resource "aws_elastic_beanstalk_environment" "production_environment" {
  name        = "production"
  application = aws_elastic_beanstalk_application.app.name
  solution_stack_name = "64bit Amazon Linux 2023 v4.2.1 running Corretto 21"

  setting {
    namespace = "aws:ec2:vpc"
    name      = "VPCId"
    value     = aws_vpc.code_learning_api_vpc.id
  }

  setting {
    namespace = "aws:autoscaling:asg"
    name      = "MinSize"
    value     = "1"
  }

  setting {
    namespace = "aws:autoscaling:asg"
    name      = "MaxSize"
    value     = "3"
  }

  setting {
    namespace = "aws:elasticbeanstalk:environment"
    name      = "EnvironmentType"
    value     = "LoadBalanced"
  }

  setting {
    namespace = "aws:ec2:vpc"
    name      = "Subnets"
    value     = join(",", aws_db_subnet_group.code_learning_api_subnet_group.subnet_ids)
  }

  setting {
    namespace = "aws:elasticbeanstalk:command"
    name      = "IgnoreHealthCheck"
    value     = "true"
  }

  setting {
    namespace = "aws:autoscaling:launchconfiguration"
    name      = "IamInstanceProfile"
    value     = aws_iam_instance_profile.beanstalk_ec2.name
  }

  setting {
    namespace = "aws:elasticbeanstalk:application:environment"
    name      = "AWS_RDS_ENDPOINT"
    value     = var.AWS_RDS_ENDPOINT
  }

  setting {
    namespace   = "aws:elasticbeanstalk:application:environment"
    name        = "DB_USERNAME"
    value       = var.DB_USERNAME
  }

  setting {
    namespace   = "aws:elasticbeanstalk:application:environment"
    name        = "DB_PASSWORD"
    value       = var.DB_PASSWORD
  }

  setting {
    namespace   = "aws:elasticbeanstalk:application:environment"
    name        = "DB_PORT"
    value       = var.DB_PORT
  }
}
