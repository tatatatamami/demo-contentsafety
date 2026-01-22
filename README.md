# demo-contentsafety

## 概要

Azure AI Content Safetyのデモアプリケーション。
チーム名を登録するUIを提供します。
Azure AI Content Safety を使用して、不適切なチーム名をフィルタリングします。

## 必要な環境

- .NET 10 SDK 以上
- Azure AI Content Safety リソース（オプション：未設定の場合はバリデーションをスキップします）

## Azure AI Content Safety の設定

### 1. Azure リソースの作成

1. Azure Portal で「Azure AI Content Safety」リソースを作成
2. リソースの「キーとエンドポイント」ページから以下を取得：
   - **エンドポイント**: `https://your-resource.cognitiveservices.azure.com/`
   - **キー**: `your-key-here`

### 2. 設定方法（User Secrets 推奨）

#### Option A: User Secrets を使用（推奨、開発環境向け）

```bash
cd /path/to/demo-contentsafety

# User Secrets の初期化
dotnet user-secrets init

# Endpoint と Key を設定
dotnet user-secrets set "AzureContentSafety:Endpoint" "https://your-resource.cognitiveservices.azure.com/"
dotnet user-secrets set "AzureContentSafety:Key" "your-key-here"
```

#### Option B: appsettings.json を直接編集（本番環境では非推奨）

`appsettings.json` を編集：

```json
{
  "AzureContentSafety": {
    "Endpoint": "https://your-resource.cognitiveservices.azure.com/",
    "Key": "your-key-here"
  }
}
```

**注意**: appsettings.json にキーを記載する場合は、Git にコミットしないよう注意してください。

#### Option C: 環境変数を使用

```bash
export AzureContentSafety__Endpoint="https://your-resource.cognitiveservices.azure.com/"
export AzureContentSafety__Key="your-key-here"
```

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
  - 空文字はNG（UI バリデーション）
  - 最大23文字まで（UI バリデーション）
  - Azure AI Content Safety による内容チェック（サーバーサイド）
- 登録成功時は画面に「登録しました」と表示されます
- 登録失敗時は理由とカテゴリ別のseverityが表示されます
- 登録済みチーム名の一覧が画面下部に表示されます（メモリ保持）

### Content Safety の判定基準

Azure AI Content Safety は以下の4つのカテゴリで内容を評価します：
- **Hate** (ヘイト): 差別的な内容
- **Sexual** (性的): 性的な内容
- **Violence** (暴力): 暴力的な内容
- **SelfHarm** (自傷): 自傷行為に関する内容

各カテゴリのseverity（重大度）は 0～7 で評価され、**3以上**の場合に登録が拒否されます。

## 動作確認

### OK（登録される）ケース
- 正常なチーム名: `Red Dragons`, `Blue Team`, `Tech Warriors`

### NG（登録されない）ケースの例
- 不適切な言葉を含むチーム名（ヘイトスピーチ、暴力的表現など）
- 登録が拒否された場合、画面にカテゴリ別のseverityが表示されます
  - 例: `Hate:4, Sexual:0, Violence:0, SelfHarm:0`

### UI バリデーションのテスト
- 空文字入力 → 「チーム名を入力してください」と表示
- 24文字以上入力 → 「チーム名は23文字以内で入力してください」と表示

## トラブルシューティング

### Content Safety が設定されていない場合
- アプリケーションは正常に起動しますが、Content Safety のチェックはスキップされます
- ログに警告メッセージが出力されます: `Azure Content Safety is not configured. Validation will be bypassed.`

### API 呼び出しエラー
- ネットワークエラーや認証エラーが発生した場合、「判定に失敗しました。時間をおいて再試行してください」と表示されます
- エンドポイントとキーが正しく設定されているか確認してください

## 参考資料

- [Azure AI Content Safety .NET SDK](https://learn.microsoft.com/en-us/dotnet/api/overview/azure/ai.contentsafety-readme)
- [Harm categories](https://learn.microsoft.com/ja-jp/azure/ai-services/content-safety/concepts/harm-categories)
- [Pricing (Text records)](https://azure.microsoft.com/en-us/pricing/details/content-safety/)
