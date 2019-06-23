using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace jtbPromise
{
    public class DownloadPageViewModel : INotifyPropertyChanged
    {
        public DownloadPageViewModel()
        {
            

        }

        public event PropertyChangedEventHandler PropertyChanged;

        public string DocImagePath
        {
            get
            {
                return DocImagePath;
            }
            set
            {
                if(DocImagePath != value)
                {
                    DocImagePath = value;
                    OnPropertyChanged("DocImagePath");
                }
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
