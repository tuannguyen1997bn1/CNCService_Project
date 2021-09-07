
namespace CNCService
{
    public partial class StatusVM : BaseVM
    {
        private static StatusVM _instance = null;
        public static StatusVM instance
        {
            get
            {
                if (_instance == null)
                    _instance = new StatusVM();
                return _instance;
            }
        }
        private static StatusVM _instance1 = null;
        public static StatusVM instance1
        {
            get
            {
                if (_instance1 == null)
                    _instance1 = new StatusVM();
                return _instance1;
            }
        }

        public enum Machine3State
        {
            Running, Stopping, Falling, Waiting
        }
        public enum MachineDoor2Status
        {
            Openning, Closing
        }
        private MachineDoor2Status _statedoor;
        public MachineDoor2Status statedoor
        {
            get
            {
                return _statedoor;
            }
            set
            {
                _statedoor = value;
                OnPropertyChanged("statedoor");
            }
        }
        private Machine3State _machinestate;
        public Machine3State machinestate
        {
            get
            {
                return _machinestate;
            }
            set
            {
                _machinestate = value;
                OnPropertyChanged("machinestate");
            }
        }
        public static void CheckState(Wise1 wise)
        {
            int[] rs = { 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach (var str in wise.DIVal)
            {
                if (str.Stat == 1)
                {
                    switch (str.Ch)
                    {
                        case 0:
                            instance.machinestate = Machine3State.Running;
                            rs[0] = 1;
                            break;
                        case 1:
                            instance.machinestate = Machine3State.Stopping;
                            rs[1] = 1;
                            break;
                        case 2:
                            instance.machinestate = Machine3State.Falling;
                            rs[2] = 1;
                            break;
                        case 3:
                            instance.statedoor = MachineDoor2Status.Closing;
                            rs[3] = 1;
                            break;
                        case 4:
                            instance1.machinestate = Machine3State.Running;
                            rs[4] = 1;
                            break;
                        case 5:
                            instance1.machinestate = Machine3State.Stopping;
                            rs[5] = 1;
                            break;
                        case 6:
                            instance1.machinestate = Machine3State.Falling;
                            rs[6] = 1;
                            break;
                        case 7:
                            instance1.statedoor = MachineDoor2Status.Closing;
                            rs[7] = 1;
                            break;
                    }
                }
                else
                {
                    if (str.Ch == 3)
                    {
                        instance.statedoor = MachineDoor2Status.Openning;
                    }
                    if (str.Ch == 7)
                    {
                        instance1.statedoor = MachineDoor2Status.Openning;
                    }
                }
            }
            if (rs[0] == 0 && rs[1] == 0 && rs[2] == 0)
            {
                instance.machinestate = Machine3State.Waiting;
            }
            if (rs[4] == 0 && rs[5] == 0 && rs[6] == 0)
            {
                instance1.machinestate = Machine3State.Waiting;
            }
        }
    }
}

