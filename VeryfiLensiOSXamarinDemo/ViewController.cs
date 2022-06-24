using VeryfiLensiOS;
using Foundation;
using System;
using UIKit;

namespace VeryfiLensiOSXamarinDemo
{
    public partial class ViewController : UIViewController
    {
        const string CLIENT_ID = "XXX";
        const string AUTH_USRNE = "XXX";
        const string AUTH_API_K = "XXX";
        const string API_URL = "XXX";

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            SetUpVeryfiLens();
            SetUpVeryfiLensDelegate();
        }

        partial void OpenLens(UIButton sender)
        {
            VeryfiLens.Shared.ShowCameraIn(this);
        }

        private void SetUpVeryfiLensDelegate()
        {
            VeryfiLens.Shared.Delegate = new VeryfiLensDelegateListener(this);
        }

        private void SetUpVeryfiLens()
        {
            var categories = new string[]
            {
                "Meals",
                "Entertainment",
                "Supplies"
            };
            var documentTypes = new string[]
            {
                "long_receipt",
                "receipt",
                "bill",
                "other"
            };
            VeryfiLensSettings veryfiLensSettings = new VeryfiLensSettings
            {
                ShowDocumentTypes = true,
                BlurDetectionIsOn = true,
                AutoLightDetectionIsOn = false,
                AutoCaptureIsOn = false,
                BackupDocsToGallery = true,
                AutoDocDetectionAndCropIsOn = true,
                AutoCropGalleryIsOn = false,
                EmailCCIsOn = false,
                ConfidenceDetailsIsOn = true,
                CloseCameraOnSubmit = true,
                RotateDocIsOn = true,
                ReturnStitchedPDF = true,
                StitchIsOn = true,
                MultipleDocumentsIsOn = true,
                LocationServicesIsOn = true,
                MoreMenuIsOn = true,
                DataExtractionEngine = DataExtractionEngine.VeryfiCloudAPI,
                ShieldProtectionIsOn = true,
                Categories = categories,
                IsProduction = false,
                OriginalImageMaxSizeInMB = 2.5f,
                StitchedPDFPixelDensityMultiplier = 2.0f,
                SaveLogIsOn = true,
                ShareLogsIsOn = true,
                DocumentTypes = documentTypes,
                AutoSubmitDocumentOnCapture = false,
                AutoRotateIsOn = true,
                ExternalId = "testExternalId1234",
                GalleryIsOn = true
            };
            var veryfiLensCredentials = new VeryfiLensCredentials
            (
             CLIENT_ID,
             AUTH_USRNE,
             AUTH_API_K,
             API_URL
            );
            VeryfiLens.Shared.ConfigureWith(veryfiLensCredentials, veryfiLensSettings, null);
            SetCategories();
            SetCustomers();
            SetTags();
            SetCostCodes();

        }

        private void SetCostCodes()
        {
            var costCodes = new VeryfiLensCostCode[10];
            for (int i = 0; i < 10; i++)
            {
                var job = new VeryfiLensCostCode
                {
                    CostCodeId = i,
                    Code = "00" + i,
                    Name = "Cost " + i
                };
                costCodes[i] = job;
            }
            VeryfiLens.Shared.Settings.CostCodes = costCodes;
        }

        private void SetTags()
        {
            var tags = new VeryfiLensTag[10];
            for (int i = 0; i < 10; i++)
            {
                var tag = new VeryfiLensTag
                {
                    TagId = i,
                    Name = "Tag Number " + i
                };
                tags[i] = tag;
            }
            VeryfiLens.Shared.Settings.Tags = tags;
        }

        private void SetCustomers()
        {
            var customers = new VeryfiLensCPModel[10];
            for (int i = 0; i < 10; i++)
            {
                var customer = new VeryfiLensCPModel
                {
                    ProjectId = i,
                    CustomerId = i,
                    CustomerName = "Customer " + i
                };
                customers[i] = customer;
            }
            VeryfiLens.Shared.Settings.Customers = customers;
        }

        private void SetCategories()
        {
            var categories = new VeryfiLensCategory[10];
            for (int i = 0; i < 10; i++)
            {
                VeryfiLensCategory category = new VeryfiLensCategory
                {
                    CategoryId = i,
                    Name = "Category Number " + i,
                    Type = "mo_expense"
                };
                categories[i] = category;
            }
            VeryfiLens.Shared.Settings.CategoriesList = categories;
        }

        private void ShowLogs(string log)
        {
            var text = Logs.Text + log + "\n";
            Logs.Text = text;
        }

        private class VeryfiLensDelegateListener : VeryfiLensDelegate
        {
            private ViewController viewController;

            public VeryfiLensDelegateListener(ViewController viewController)
            {
                this.viewController = viewController;
            }

            public override void VeryfiLensClose(NSDictionary<NSString, NSObject> json)
            {
                viewController.ShowLogs(json.ToString());
            }

            public override void VeryfiLensError(NSDictionary<NSString, NSObject> json)
            {
                viewController.ShowLogs(json.ToString());
            }

            public override void VeryfiLensSuccess(NSDictionary<NSString, NSObject> json)
            {
                viewController.ShowLogs(json.ToString());
            }

            public override void VeryfiLensUpdate(NSDictionary<NSString, NSObject> json)
            {
                viewController.ShowLogs(json.ToString());
            }
        }

    }
}
