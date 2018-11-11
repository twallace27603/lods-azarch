apt update
apt install -y docker.io docker-compose
groupadd docker
gpasswd -a student docker
