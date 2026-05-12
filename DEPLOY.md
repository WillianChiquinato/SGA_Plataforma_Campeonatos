# Deploy Guide - SGA Plataforma API

## Checklist Pre-Deploy

- [ ] Migrations criadas e testadas localmente
- [ ] `.env` configurado com variáveis de produção
- [ ] Banco de dados PostgreSQL criado na Contabo
- [ ] Certificado SSL/TLS configurado (para HTTPS)
- [ ] Backup do banco de dados configurado

## Variáveis de Ambiente Obrigatórias

Copie o `.env.example` para `.env` e configure com valores reais:

```bash
# Database
DB_SERVER=seu-banco.contabo.com
DB_PORT=5432
DB_NAME=sga_db
DB_USER=seu_usuario
DB_PASSWORD=senha_forte_aqui
DB_SSL=true  # Use true em produção

# JWT (gere uma chave segura)
JWT_KEY=gere-uma-chave-aleatoria-com-minimo-32-caracteres
JWT_ISSUER=https://sua-api-url.com
JWT_AUDIENCE=https://sua-api-url.com

# CORS
CORS_ORIGIN=https://seu-frontend.com
```

## Deploy com Docker na Contabo

### 1. Build da imagem

```bash
docker build -t sga-api:latest .
```

### 2. Run em produção

```bash
docker run -d \
  --name sga-api \
  -p 8080:8080 \
  --env-file .env \
  --restart unless-stopped \
  sga-api:latest
```

### 3. Ou use docker-compose

```bash
docker-compose up -d
```

## Configuração de Proxy Reverso (Nginx/Traefik)

Se usar Nginx para gerenciar HTTPS e domínio:

```nginx
server {
    listen 443 ssl http2;
    server_name sua-api.com;
    
    ssl_certificate /path/to/cert.pem;
    ssl_certificate_key /path/to/key.pem;
    
    location / {
        proxy_pass http://localhost:8080;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

## Verificar Status

```bash
# Ver logs
docker logs -f sga-api

# Health check
curl http://localhost:8080/

# Swagger (apenas desenvolvimento)
curl http://localhost:8080/swagger/index.html
```

## Migrations em Produção

As migrations rodam automaticamente no startup. Se precisar rodar manualmente:

```bash
docker exec sga-api dotnet ef database update --project SGA_Plataforma.Api
```

## Troubleshooting

### Erro de conexão com banco
- Verifique `DB_SERVER`, `DB_PORT`, credenciais
- Certifique-se que o firewall permite conexões TCP 5432
- Teste: `docker exec sga-api curl -f http://localhost:8080/`

### Erro JWT/CORS
- Confirme que `JWT_KEY` tem no mínimo 32 caracteres
- Verifique `CORS_ORIGIN` sem trailing slash

### Banco de dados vazio após deploy
- Verifique se as migrations rodaram: `docker logs sga-api | grep Migrate`
- Se não, a aplicação pode não ter permissão de schema alteration

## Monitoramento Recomendado

- [ ] Logs centralizados (Datadog, ELK, etc)
- [ ] Alertas para erros HTTP 5xx
- [ ] Monitoramento de CPU/RAM do container
- [ ] Backup diário do PostgreSQL
- [ ] Rate limiting já está ativo (100 req/min por IP)
