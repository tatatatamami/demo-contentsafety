# demo-contentsafety

## 概要

Azure AI Content Safetyのデモアプリケーション。
チーム名を登録するUIを提供します。

## 必要な環境

- .NET 8 SDK 以上

## ローカル起動手順

1. リポジトリをクローン
```bash
git clone https://github.com/tatatatamami/demo-contentsafety.git
cd demo-contentsafety
```

2. アプリケーションの実行
```bash
dotnet run
```

3. ブラウザでアクセス
- デフォルトURL: https://localhost:5001 または http://localhost:5000
- コンソールに表示されるURLを確認してアクセスしてください

## 機能

### チーム名登録画面
- チーム名を入力して登録できます
- バリデーション：
  - 空文字はNG
  - 最大23文字まで
- 登録成功時は画面に「登録しました」と表示されます
- 登録済みチーム名の一覧が画面下部に表示されます（メモリ保持）
