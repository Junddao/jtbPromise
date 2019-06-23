using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace jtbPromise
{
    public class MakeDocPageViewModel : INotifyPropertyChanged
    {
        string title = string.Empty;
        string firstPersonName = string.Empty;
        string secondPersonName = string.Empty;
        string writeContent = string.Empty;
        string document = string.Empty;

        public string FirstPersonName
        {
            get
            {
                return firstPersonName;
            }
            set
            {
                if (firstPersonName != value)
                {
                    firstPersonName = value;
                    document = "(갑) " + firstPersonName + "은\n(을)" + secondPersonName + "에게 \n " + writeContent;
                    OnPropertyChanged("FirstPersonName");
                    OnPropertyChanged("Document");
                }
            }
        }

        public string SecondPersonName
        {
            get
            {
                return secondPersonName;
            }
            set
            {
                if (secondPersonName != value)
                {
                    secondPersonName = value;
                    document = "(갑) " + firstPersonName + "은\n(을)" + secondPersonName + "에게 \n" + writeContent;
                    OnPropertyChanged("SecondPersonName");
                    OnPropertyChanged("Document");
                }
            }
        }


        public string WriteContent
        {
            get
            {
                return writeContent;
            }
            set
            {
                if (writeContent != value)
                {
                    writeContent = value;
                    document = "(갑) " + firstPersonName + "은\n(을)" + secondPersonName + "에게 \n" + writeContent;
                    OnPropertyChanged("Document");
                }
            }
        }


        public string Document
        {
            get
            {
                return document;
            }
            set
            {

            }
        }

        public MakeDocPageViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
