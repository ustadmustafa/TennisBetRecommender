# Docker başlatma scripti
Write-Host "TennisBets uygulaması Docker ile başlatılıyor..." -ForegroundColor Green

# Docker Compose ile uygulamayı başlat
docker-compose up --build

Write-Host "Uygulama başlatıldı!" -ForegroundColor Green
Write-Host "HTTP: http://localhost:8080" -ForegroundColor Yellow
Write-Host "HTTPS: https://localhost:8081" -ForegroundColor Yellow
Write-Host "Durdurmak için: docker-compose down" -ForegroundColor Cyan
