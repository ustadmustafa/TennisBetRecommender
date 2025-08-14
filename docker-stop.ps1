# Docker durdurma scripti
Write-Host "TennisBets uygulaması durduruluyor..." -ForegroundColor Yellow

# Docker Compose ile uygulamayı durdur
docker-compose down

Write-Host "Uygulama durduruldu!" -ForegroundColor Green
Write-Host "Tüm container'ları ve image'ları temizlemek için: docker-compose down --rmi all" -ForegroundColor Cyan
