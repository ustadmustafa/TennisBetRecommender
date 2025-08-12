# 🎾 Tennis Betting Prediction System

Bu proje, MVC mimarisinde C# ile geliştirilmiş kural tabanlı tenis bahis tahmin sistemidir. API endpoint'lerinden gelen verileri analiz ederek çeşitli bahis türleri için tahminler üretir.

## 🚀 Özellikler

### Bahis Türleri
1. **Maç Kazananı** - H2H geçmiş + oyuncu sıralaması
2. **Toplam Set** - H2H maç set analizi
3. **İlk Set Kazananı** - H2H ilk set + sezon performansı
4. **Handikap Bahsi** - H2H skor farkları + performans analizi
5. **Comeback Bahsi** - İlk set kaybeden oyuncunun maç kazanma oranı

### Veri Kaynakları
- **H2H (Head-to-Head)** - İki oyuncu arasındaki geçmiş maçlar
- **Standings** - Oyuncuların dünya sıralaması ve puanları
- **Player Stats** - Oyuncuların sezonluk performans istatistikleri

## 🏗️ Proje Yapısı

```
TennisBets/
├── Controllers/
│   └── TennisController.cs          # Ana controller + bahis tahmin endpoint'leri
├── Models/
│   ├── TennisMatch.cs               # Mevcut maç modeli
│   ├── ApiModels.cs                 # API response modelleri
│   └── BettingModels.cs             # Bahis tahmin modelleri
├── Services/
│   ├── ITennisService.cs            # Mevcut tenis servisi interface'i
│   ├── ITennisApiService.cs         # API çağrıları interface'i
│   ├── TennisApiService.cs          # API çağrıları implementasyonu
│   ├── IBettingPredictionService.cs # Bahis tahmin servisi interface'i
│   └── BettingPredictionService.cs  # Kural tabanlı tahmin algoritmaları
└── Views/
    └── Tennis/
        └── Index.cshtml             # Maç listesi + bahis önerileri butonu
```

## 🔧 Kurulum

1. **API Key Ayarlama**
   ```json
   // appsettings.json
   {
     "TennisApi": {
       "ApiKey": "your_actual_api_key_here"
     }
   }
   ```

2. **Proje Çalıştırma**
   ```bash
   dotnet run
   ```

3. **Tarayıcıda Açma**
   ```
   https://localhost:5001
   ```

## 📊 Kural Tabanlı Algoritmalar

### 1. Maç Kazananı Tahmini
- **H2H Ağırlığı**: %60
- **Sıralama Ağırlığı**: %40
- **Güven Hesaplama**: H2H galibiyet oranı farkı + sıralama farkı

### 2. Toplam Set Tahmini
- **Veri Kaynağı**: H2H maç geçmişi
- **Sınırlama**: API'de set skorları mevcut değil
- **Varsayılan Değer**: 2.5 set

### 3. İlk Set Kazananı
- **H2H Ağırlığı**: %70
- **Performans Ağırlığı**: %30
- **Veri Kaynağı**: H2H ilk set + sezon performansı

### 4. Handikap Bahsi
- **H2H Ağırlığı**: %60
- **Performans Ağırlığı**: %40
- **Hesaplama**: Galibiyet oranı farkı + sezon performans farkı

### 5. Comeback Bahsi
- **Veri Sınırı**: İlk set skorları API'de mevcut değil
- **Öneri**: Bu bahis türü için yetersiz veri

## 🎯 Kullanım

1. **Ana Sayfa**: Canlı ve yaklaşan maçları görüntüle
2. **Betting Tips Butonu**: Her maç için 🎯 butonuna tıkla
3. **Modal Açılır**: Bahis tahminleri yüklenir
4. **Sonuçları İncele**: Her bahis türü için tahmin, güven oranı ve öneri

## 📈 Güven Seviyeleri

- **Strong Bet** (≥80%): Güçlü bahis önerisi
- **Moderate Bet** (≥60%): Orta seviye bahis önerisi  
- **Weak Bet** (≥40%): Zayıf bahis önerisi
- **Avoid Bet** (<40%): Bahis yapmaktan kaçın

## 🔍 API Endpoint'leri

### H2H (Head-to-Head)
```
GET /Tennis/GetBettingPredictions?player1Key={id}&player2Key={id}
```

### H2H Analiz
```
GET /Tennis/GetH2HAnalysis?player1Key={id}&player2Key={id}
```

## ⚠️ Sınırlamalar

1. **Set Skorları**: API'de detaylı set skorları mevcut değil
2. **Tie-Break**: Tie-break bilgisi bulunmuyor
3. **İlk Set Analizi**: İlk set sonuçları için yetersiz veri
4. **Zemin Performansı**: Sadece hard ve clay court verisi mevcut

## 🚀 Geliştirme Önerileri

1. **Makine Öğrenmesi**: Kural tabanlı sistem yerine ML modeli
2. **Daha Fazla Veri**: Set skorları, tie-break, ace sayısı vb.
3. **Canlı Veri**: Maç sırasında güncellenen tahminler
4. **Kullanıcı Geri Bildirimi**: Tahmin doğruluğu takibi
5. **Portfolio Yönetimi**: Risk yönetimi ve bankroll stratejisi

## 📝 Lisans

Bu proje eğitim amaçlı geliştirilmiştir. Gerçek bahis amaçlı kullanımda dikkatli olunuz.

## 🤝 Katkıda Bulunma

1. Fork yapın
2. Feature branch oluşturun (`git checkout -b feature/amazing-feature`)
3. Commit yapın (`git commit -m 'Add amazing feature'`)
4. Push yapın (`git push origin feature/amazing-feature`)
5. Pull Request oluşturun

## 📞 İletişim

Proje ile ilgili sorularınız için issue açabilirsiniz.
