using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetWeaverServer.Datastructure;
using NetWeaverServer.Datastructure.Arguments;
using NetWeaverServer.Main;
using NetWeaverServer.MQTT;

namespace NetWeaverServer.Tasks
{
    public class DeploymentManager
    {
        private List<Client> Clients { get; }
        private List<Client> Copied { get; } = new List<Client>();

        private string Args { get; }

        /// <summary>
        /// Communicate with the Client
        /// </summary>
        private MqttMaster Mqtt { get; }

        /// <summary>
        /// Means to Communicate/Report back to the GUI
        /// </summary>
        private IProgress<TaskProgress> TaskProgress { get; }
        //Collect Job Reports
        private TaskProgress Progress = new TaskProgress();

        public DeploymentManager(TaskDetails details, MqttMaster mqtt)
        {
            Clients =  new List<Client>(details.Clients);
            TaskProgress = details.TaskProgress;
            Args = details.Args;
            Mqtt = mqtt;
        }

        /// <summary>
        /// //TODO: Brain me Deloyment
        /// </summary>
        public async Task DeployForAllClients()
        {
            //First is the Choosen One
            Client thechoosenone = Clients.ElementAt(0);
            /*JobProgress jobProgress = new JobProgress(thechoosenone);
            jobProgress.ProgressChanged += HandleJobProgressReport;
            Progress.JobProgress.Add(jobProgress);*/
            Copied.Add(thechoosenone);
            Clients.RemoveAt(0);

            //await new CopyFileJob(new ClientChannel(thechoosenone, Mqtt), jobProgress, Args).Work();

            while (Clients.Count > 0)
            {
                Console.WriteLine($"List of Copied Clients {Copied.Select(x => x.HostName).ToList().ToFormat()}");
                List<Task> tasks = new List<Task>();
                int limit = Copied.Count;

                for(int i = 0; i < limit; i++)
                {
                    if (Clients.Count > 0)
                    {
                        /*
                        Client client = Copied.ElementAt(i);
                        JobProgress jp = new JobProgress(client);
                        jp.ProgressChanged += HandleJobProgressReport;
                        tasks.Add(
                            new DistributeFileJob(new ClientChannel(client, Mqtt), jp,
                                new ClientChannel(Clients.ElementAt(1), Mqtt), Args).Work()
                        );
                        */
                        Console.WriteLine($"Deploying from: {Copied.ElementAt(i).HostName} to {Clients.ElementAt(0).HostName}");
                        Copied.Add(Clients.ElementAt(0));
                        Clients.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }

                await Task.WhenAll(tasks);
            }

            Console.WriteLine("Done");

        }

        private void HandleJobProgressReport(object sender, EventArgs e)
        {
            TaskProgress.Report(Progress);
        }
    }
}