using ApiApp.Models;
using ApiApp.Services.StorageAreaService;
using InterWMSDesctop.Services.DialogService;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace InterWMSDesctop.ViewModels
{
    class StorageAreaVM : ViewModelBase
    {
        #region Fields
        private readonly IStorageAreaService _storageAreaService;
        private readonly IDialogService _dialogService;

        private IEnumerable<StorageArea> _storageAreas;
        private StorageArea _selectedStorageArea;
        #endregion

        #region Constructor
        public StorageAreaVM(IStorageAreaService storageAreaService,
                             IDialogService dialogService)
        {
            _storageAreaService = storageAreaService;
            _dialogService = dialogService;
        }
        #endregion

        #region Properties
        public IEnumerable<StorageArea> StorageAreas => _storageAreas;

        public StorageArea SelectedStorageArea
        {
            get => _selectedStorageArea;
            set => OnPropertyChanged(ref _selectedStorageArea, value, () => SelectedStorageArea);
        }
        #endregion

        #region Public methods
        public override async Task Load()
        {
            _storageAreas = await _storageAreaService.GetStorageAreas();
            OnPropertyChanged(() => StorageAreas);
        }
        #endregion

        #region Commands
        #region AddCmd
        private ICommand _addCmd;

        public ICommand AddCmd
            => _addCmd ?? (_addCmd = new AsyncCommand(Add, CanAdd));

        private bool CanAdd(object obj)
        {
            return true;
        }

        private async Task Add(object obj)
        {
            var location = _dialogService.InputDialog("Создание новой зоны", "");

            if (string.IsNullOrWhiteSpace(location))
            {
                return;
            }

            var result = await _storageAreaService.AddStorageArea(new StorageArea { Location = location });

            if (result != null)
            {
                await Load();
            }
        }
        #endregion

        #region DeleteCmd
        private ICommand _deleteCmd;

        public ICommand DeleteCmd
            => _deleteCmd ?? (_deleteCmd = new AsyncCommand(Delete));

        private async Task Delete(object obj)
        {
            if (obj is StorageArea storageArea)
            {
                var result = await _storageAreaService.DeleteStorageArea(storageArea.Id);
                if (result)
                {
                    await Load();
                }
            }
        }
        #endregion

        #region EditCmd
        private ICommand _editCmd;

        public ICommand EditCmd
            => _editCmd ?? (_editCmd = new AsyncCommand(Edit));

        private async Task Edit(object obj)
        {
            if (SelectedStorageArea != null)
            {
                var result = await _storageAreaService.EditStorageArea(SelectedStorageArea);
                if (result != null)
                {
                    await Load();
                    SelectedStorageArea = null;
                }
            }
        }
        #endregion
        #endregion

        #region Private methods

        #endregion
    }
}
