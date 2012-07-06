stsadm -o addsolution -filename Siderys.Search.Managed.Metadata.wsp
echo *
echo * Activando la solucion Siderys.Search.Managed.Metadata.wsp
echo *
stsadm -o deploysolution -name Siderys.Search.Managed.Metadata.wsp -immediate -allowGacDeployment
stsadm -o execadmsvcjobs
iisreset


