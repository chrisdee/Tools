echo *
echo * Desactivando el feature Siderys.Search.Managed.Metadata del sitio http://win2k8sps:2007
echo *
stsadm -o deactivatefeature -name Siderys.Search.Managed.Metadata -url http://win2k8sps:2007/ -force

 
echo *
echo * Desinstalando el feature Siderys.Search.Managed.Metadata
echo *
stsadm -o uninstallfeature -name Siderys.Search.Managed.Metadata -force


echo *
echo * Eliminando la solucion Siderys.Search.Managed.Metadata
echo *
stsadm -o deletesolution -name Siderys.Search.Managed.Metadata.wsp -override
stsadm -o execadmsvcjobs