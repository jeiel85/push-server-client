# TLS/SSL 인증서 설정 가이드

## 개요

WebSocket 서버에 TLS/SSL 인증서를 적용하여 보안을 강화합니다.

## 인증서 생성 (자체 서명 - 개발용)

### Windows (PowerShell)

```powershell
# OpenSSL 설치 필요 (https://slproweb.com/products/Win32OpenSSL.html)

# 1. 개인 키 생성
openssl genrsa -out server.key 2048

# 2. CSR 생성
openssl req -new -key server.key -out server.csr

# 3. 자체 서명 인증서 생성
openssl x509 -req -days 365 -in server.csr -signkey server.key -out server.crt

# 4. PFX 파일 생성 (IIS/ASP.NET용)
openssl pkcs12 -export -out server.pfx -inkey server.key -in server.crt
```

### Linux/macOS

```bash
# 1. 개인 키 생성
openssl genrsa -out server.key 2048

# 2. CSR 생성
openssl req -new -key server.key -out server.csr -subj "/CN=localhost"

# 3. 자체 서명 인증서 생성
openssl x509 -req -days 365 -in server.csr -signkey server.key -out server.crt

# 4. PFX 파일 생성
openssl pkcs12 -export -out server.pfx -inkey server.key -in server.crt
```

## 프로덕션 인증서 (Let's Encrypt 권장)

```bash
# Certbot 설치 후
sudo certbot certonly --standalone -d yourdomain.com

# 인증서 위치
# /etc/letsencrypt/live/yourdomain.com/fullchain.pem
# /etc/letsencrypt/live/yourdomain.com/privkey.pem
```

## 설정 적용

### 1. 인증서 파일 복사

```bash
# server.pfx 파일을 ws002/cert/ 폴더에 복사
mkdir -p ws002/cert
cp server.pfx ws002/cert/
```

### 2. appsettings.json 수정

```json
{
  "serverOptions": {
    "name": "WebSocketPushServer",
    "listeners": [
      {
        "ip": "Any",
        "port": 7000
      },
      {
        "ip": "Any",
        "port": 7443,
        "security": "Tls12",
        "certificateOptions": {
          "filePath": "cert/server.pfx",
          "password": "${TLS_CERT_PASSWORD}"
        }
      }
    ]
  }
}
```

### 3. 환경변수 설정

```bash
# Linux/macOS
export TLS_CERT_PASSWORD=your-cert-password

# Windows (PowerShell)
$env:TLS_CERT_PASSWORD = "your-cert-password"

# Windows (CMD)
set TLS_CERT_PASSWORD=your-cert-password
```

## 클라이언트 연결

### WSS 연결 (Production)

```csharp
// C# WebSocketSharp
var ws = new WebSocket("wss://yourdomain.com:7443/");

// JavaScript
var ws = new WebSocket("wss://yourdomain.com:7443/");
```

### 테스트 (자체 서명 인증서)

```csharp
// 자체 서명 인증서 무시 (개발용만)
var ws = new WebSocket();
ws.SslConfiguration.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
ws.Connect("wss://localhost:7443/");
```

## 보안 권장사항

1. **프로덕션**: Let's Encrypt 또는 상용 CA에서 인증서 발급
2. **키 관리**: PFX 비밀번호는 환경변수 또는 시크릿 매니저 사용
3. **TLS 버전**: TLS 1.2 이상만 허용
4. **인증서 갱신**: 만료 전 자동 갱신 스크립트 설정
