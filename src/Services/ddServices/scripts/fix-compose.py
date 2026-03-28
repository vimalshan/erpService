import re

with open('docker-compose.yml', 'r') as f:
    content = f.read()

# Remove depends_on blocks that contain sqlserver/rabbitmq/azurite (infra deps)
pattern = r'    depends_on:\n(      (sqlserver|rabbitmq|azurite):\n        condition: service_(healthy|started)\n)+'
content = re.sub(pattern, '', content)

with open('docker-compose.yml', 'w') as f:
    f.write(content)

print('DONE - removed infrastructure depends_on blocks')
