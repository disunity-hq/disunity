# Attempt to fix broken network in WSL2.0 
$guest_ip = bash -c "/sbin/ifconfig eth0 | egrep -o 'inet [0-9\.]+' | cut -d ' ' -f2"
Write-Output "Guest IP IS:  $guest_ip"
$gateway_ips = Get-NetIPAddress -InterfaceAlias "vEthernet (WSL)" | select IPAddress
$gateway_ip = $gateway_ips[1].IPAddress
Write-Output "Gateway (local WSL adapter) IP is: $gateway_ip"
bash -c "sudo ifconfig eth0 netmask 255.255.240.0"
bash -c "sudo ip route add default via $gateway_ip"