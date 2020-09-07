# AppleOAuth串接Sample

## 該Sample串接了兩個Apple API

* 取得PublicKey : https://appleid.apple.com/auth/keys
* 取得Token : https://appleid.apple.com/auth/token
> https://developer.apple.com/documentation/sign_in_with_apple/generate_and_validate_tokens


## 流程
* 註冊Sign in with Apple Key
 1. 在apple developer 建立一組 Sign in with Apple 的 Key，格式為p8並保存下來

* 取得identityToken
 1. 在前端 AppleOAuth 登入(IOS流程 or 正常OAuth callback流程) Apple Id 成功後會取得identityToken

* 驗證identityToken
 1. 呼叫Apple PublicKey API，取得公鑰用來驗證 identityToken 是否為 Apple加密產生的

* 取得token
 1. 取得token的參數為JWT，透過剛剛的 identityToken 以及 Apple developer 後台的相關資訊組成 JWT 參數
 2. 其JWT加密規則由 Sign in with Apple 的 Key 以 ES256 所加密
 
※ 只有在User第一次使用Apple OAuth串接，取得Token才會吐個人資訊
