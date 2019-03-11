using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace jtbPromise
{
    class CSearchFileViewModel
    {
        private ObservableCollection<CSearchFile> files = new ObservableCollection<CSearchFile>();

        public ObservableCollection<CSearchFile> Files
        {
            get
            {
                return files;
            }
            set
            {
                files = value;
            }
        }

        public CSearchFileViewModel()
        {
            Files = new ObservableCollection<CSearchFile>();
        }
    }
}
