variable "DB_USERNAME" {
  description = "Database username"
  type = string
  sensitive = true
}

variable "DB_PASSWORD" {
  description = "Database password"
  type = string
  sensitive = true
}

variable "AWS_REGION" {
  description = "Region of resources"
  type = string
  sensitive = true
}


variable "DB_PORT" {
  description = "Database port"
  type = string
  sensitive = true
}

variable "AWS_RDS_ENDPOINT" {
  description = "Database Endpoint"
  type = string
  sensitive = true
}
