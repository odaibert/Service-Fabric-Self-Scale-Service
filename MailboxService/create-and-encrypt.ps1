# Create a new self-signed certificate
<#
New-SelfSignedCertificate -Type DocumentEncryptionCert `
                          -KeyUsage DataEncipherment `
                          -Subject "CN=myallscriptsmailboxpoc" `
                          -CertStoreLocation "cert:\LocalMachine\My" `
                          -Provider 'Microsoft Enhanced Cryptographic Provider v1.0'
#>

# Set the thumbprint for the PFX certificate
$thumbprint = "59df289207a303c694ae01c0e4a84b94dab305bc"

$textToEncrypt = "fkdMzvAWTay0zn8nuE8cXjC5bc2LIEOpdsEFhCmCErMFGI3Fx2ZGb56Ao1kVmH4h36DUMBitoNYxzxpeLI13SQ=="
Invoke-ServiceFabricEncryptText -Text $textToEncrypt `
                                -CertStore `
                                -CertThumbprint $thumbprint `
                                -StoreLocation LocalMachine `
                                -StoreName My
