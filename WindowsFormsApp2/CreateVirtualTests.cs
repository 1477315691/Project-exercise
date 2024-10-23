using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.ResourceManager.Network.Models;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Redis;
using Azure.Core;
using Azure.ResourceManager.Compute.Models;
using Azure.ResourceManager.Compute;
using System.IO;

namespace WindowsFormsApp2
{
    public class CreateVirtualTests
    {
        public static VirtualNetworkResource? vnet;
        public static RedisResource cache;
        public static VirtualMachineResource? vm;
        public async static Task CreateVnetCacheTest(CacheFeatures cacheFeatures, AzureClient azureClient, string region, string caseId)
        {
            Random random = new Random();
            var randNum = random.Next(100);
            var subnetName = ("subnetmanaul" + caseId + region.ToLower()).Replace(" ", "");

            // Create vnet
            var virtualNetworkCC = azureClient.ResourceGroupResource.GetVirtualNetworks();
            var virtualNetworkParameter = new VirtualNetworkData()
            {
                Location = region,

            };
            virtualNetworkParameter.AddressPrefixes.Add("10.0.2.0/24");
            SubnetData subData = new SubnetData();
            subData.Name = subnetName;
            subData.AddressPrefix = "10.0.2.0/24";

            virtualNetworkParameter.Subnets.Add(subData);

            int itr = 0;
            while (itr < 4)
            {
                try
                {
                    vnet = (await virtualNetworkCC.CreateOrUpdateAsync(WaitUntil.Completed, ("vnetmanaul" + caseId + region.ToLower()).Replace(" ", ""), virtualNetworkParameter)).Value;
                    break;
                }
                catch (Exception ex)
                {
                    if (itr == 3) throw;
                }

                randNum = random.Next(100);
                itr++;
            }

            var options = new RedisCacheOptions()
            {
                SkuName = "Premium",
                SubnetId = vnet.Data.Subnets[0].Id,
                ForceCreate = true,
                CacheFeatures = cacheFeatures,
                CachePrefix = $"{nameof(CreateVnetCacheTest)}",
                RegionName = region
            };
            string cacheName = ("cacheManual" + caseId + region.ToLower()).Replace(" ", "");
            cache = await RedisClient.CreateRedisResource(cacheName, options, AzureClient.cli.RedisCollection);
        }
        public async static Task CreateVMInVnetAsync(CacheFeatures cacheFeatures, AzureClient azureClient, string region, string caseId)
        {
            await CreateVnetCacheTest(cacheFeatures, azureClient, region, caseId);
            string vmName = ("vmachinemanaul" + caseId + region.ToLower()).Replace(" ", "");
            var nicIpConfig = new NetworkInterfaceIPConfigurationData()
            {
                Name = "Primary",
                Primary = true,
                Subnet = new SubnetData() { Id = vnet.Data.Subnets.First().Id },
                PrivateIPAllocationMethod = NetworkIPAllocationMethod.Dynamic
            };
            var nicData = new NetworkInterfaceData()
            {
                Location = region,
            };
            nicData.IPConfigurations.Add(nicIpConfig);
            NetworkInterfaceCollection nics = azureClient.ResourceGroupResource.GetNetworkInterfaces();
            NetworkInterfaceResource nic = (await nics.CreateOrUpdateAsync(WaitUntil.Completed, vmName + "-nic", nicData)).Value;

            VirtualMachineNetworkProfile vmNetworkProfile = new VirtualMachineNetworkProfile();
            vmNetworkProfile.NetworkInterfaces.Add(new VirtualMachineNetworkInterfaceReference()
            {
                Id = nic.Id,
                DeleteOption = ComputeDeleteOption.Delete
            });
            VirtualMachineData vmd = new VirtualMachineData(region)
            {
                NetworkProfile = vmNetworkProfile,
                OSProfile = new VirtualMachineOSProfile()
                {
                    ComputerName = vmName,
                    AdminUsername = "azureuser",
                    AdminPassword = "1234abcABC"
                },
                HardwareProfile = new VirtualMachineHardwareProfile()
                {
                    VmSize = VirtualMachineSizeType.StandardD2V3
                },
                StorageProfile = new VirtualMachineStorageProfile()
                {
                    OSDisk = new VirtualMachineOSDisk(DiskCreateOptionType.FromImage)
                    {
                        DeleteOption = DiskDeleteOptionType.Delete
                    },
                    ImageReference = new ImageReference()
                    {
                        Offer = "0001-com-ubuntu-minimal-focal",
                        Publisher = "canonical",
                        Sku = "minimal-20_04-lts",
                        Version = "latest"
                    }
                }
            };
            VirtualMachineCollection vmc = azureClient.ResourceGroupResource.GetVirtualMachines();
            vm = (await vmc.CreateOrUpdateAsync(WaitUntil.Completed, vmName, vmd)).Value;
            RunCommandInput input = new RunCommandInput("RunShellScript");
            input.Script.Add("sudo apt-get update && sudo apt-get install software-properties-common -y -q && sudo add-apt-repository universe");
            input.Script.Add($"sudo apt-get install redis-tools -y -q");
            VirtualMachineRunCommandResult result = (await vm.RunCommandAsync(WaitUntil.Completed, input)).Value;
        }

    }
}
