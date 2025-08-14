# Docker ile TennisBets Uygulaması

Bu proje Docker kullanarak çalıştırılabilir.

## Gereksinimler

- Docker Desktop
- Docker Compose

## Hızlı Başlangıç

### 1. Uygulamayı Build Etme ve Çalıştırma

```bash
# Proje dizininde
docker-compose up --build
```

### 2. Sadece Çalıştırma (Önceden build edilmişse)

```bash
docker-compose up
```

### 3. Arka Planda Çalıştırma

```bash
docker-compose up -d
```

### 4. Uygulamayı Durdurma

```bash
docker-compose down
```

## Erişim

- HTTP: http://localhost:8080
- HTTPS: https://localhost:8081

## Geliştirme Modu

Geliştirme sırasında kod değişikliklerinin otomatik olarak yansıması için:

```bash
docker-compose up --build
```

## Docker Image'ını Manuel Olarak Build Etme

```bash
docker build -t tennisbets -f TennisBets/Dockerfile .
```

## Container'ı Manuel Olarak Çalıştırma

```bash
docker run -p 8080:8080 -p 8081:8081 tennisbets
```

## Logları Görüntüleme

```bash
docker-compose logs -f tennisbets
```

## Container'ı Temizleme

```bash
# Tüm container'ları durdur ve sil
docker-compose down

# Image'ları da sil
docker-compose down --rmi all

# Volume'ları da sil
docker-compose down --volumes --rmi all
```
