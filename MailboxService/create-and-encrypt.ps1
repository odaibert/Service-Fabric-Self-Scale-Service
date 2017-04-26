# Create a new self-signed certificate
<#
New-SelfSignedCertificate -Type DocumentEncryptionCert `
                          -KeyUsage DataEncipherment `
                          -Subject "CN=myallscriptsmailboxpoc" `
                          -CertStoreLocation "cert:\LocalMachine\My" `
                          -Provider 'Microsoft Enhanced Cryptographic Provider v1.0'
#>

# Set the thumbprint for the PFX certificate
$thumbprint = "[thumbprint]"

$textToEncrypt = "[textToEncrypt]"
Invoke-ServiceFabricEncryptText -Text $textToEncrypt `
                                -CertStore `
                                -CertThumbprint $thumbprint `
                                -StoreLocation LocalMachine `
                                -StoreName My
