$cred = Get-Credential -Message "Enter password for certificate." -UserName foo

$file = ( Get-ChildItem -Path .\ThingOne.pfx )
$file | Import-PfxCertificate -CertStoreLocation cert:\LocalMachine\My -Password $cred.Password