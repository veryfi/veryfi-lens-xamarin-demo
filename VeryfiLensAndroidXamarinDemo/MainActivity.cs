using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.AppCompat.App;
using System.Collections.Generic;
using Com.Veryfi.Lens;
using Com.Veryfi.Lens.Models;
using Org.Json;

namespace VeryfiLensAndroidXamarinDemo
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        const string CLIENT_ID = "XXX";
        const string AUTH_USRNE = "XXX";
        const string AUTH_API_K = "XXX";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            SetUpVeryfiLens();
            SetUpVeryfiLensDelegate();

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var fab = FindViewById<Android.Widget.Button>(Resource.Id.fab);
            fab.Click += FabOnClick;
        }

        private void SetUpVeryfiLensDelegate()
        {
            VeryfiLens.SetDelegate(new VeryfiLensDelegateListener(this));
        }

        private void SetUpVeryfiLens()
        {
            var categories = new List<string>
            {
                "Meals",
                "Entertainment",
                "Supplies"
            };
            var documentTypes = new List<string>
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
                ConfidenceDetailsIsOn = true,
                AutoCaptureIsOn = false,
                BackupDocsToGallery = true,
                AutoDocDetectionAndCropIsOn = true,
                AutoCropGalleryIsOn = false,
                EmailCCIsOn = false,
                CloseCameraOnSubmit = true,
                RotateDocIsOn = true,
                StitchIsOn = true,
                MultipleDocumentsIsOn = true,
                LocationServicesIsOn = true,
                MoreMenuIsOn = true,
                ShieldProtectionIsOn = true,
                FloatMenu = true,
                Categories = categories,
                PrimaryColor = "#53BF8A",
                AccentColor = "#8B229D",
                Production = false,
                OriginalImageMaxSizeInMB = new Java.Lang.Float(2.5),
                StitchedPDFPixelDensityMultiplier = new Java.Lang.Float(2.0),
                SaveLogsIsOn = true,
                ShareLogsIsOn = true,
                GpuIsOn = true,
                DataExtractionEngine = VeryfiLensSettings.ExtractionEngine.VeryfiCloudAPI,
                DocumentTypes = documentTypes,
                AutoSubmitDocumentOnCapture = false,
                AutoRotateIsOn = false,
                ExternalId = "testExternalId1234",
                BrandImage = new Java.Lang.Integer(Resource.Drawable.ic_veryfi_lens_logo),
                GalleryIsOn = false
            };
            var veryfiLensCredentials = new VeryfiLensCredentials
            {
                ApiKey = AUTH_API_K,
                Username = AUTH_USRNE,
                ClientId = CLIENT_ID
            };
            VeryfiLens.Configure(Application, veryfiLensCredentials, veryfiLensSettings);
            SetCategories();
            SetCustomers();
            SetTags();
            SetCostCodes();

        }

        private void SetCostCodes()
        {
            var costCodes = new List<Job>();
            for (int i = 0; i < 10; i++)
            {
                var job = new Job
                {
                    Id = new Java.Lang.Integer(i),
                    Code = "00" + i,
                    Name = "Cost " + i
                };
                costCodes.Add(job);
            }
            VeryfiLens.Settings.CostCodes = costCodes;
        }

        private void SetTags()
        {
            var tags = new List<Tag>();
            for (int i = 0; i < 10; i++)
            {
                var tag = new Tag
                {
                    Id = new Java.Lang.Integer(i),
                    Name = "Tag Number " + i
                };
                tags.Add(tag);
            }
            VeryfiLens.Settings.Tags = tags;
        }

        private void SetCustomers()
        {
            var customers = new List<CustomerProject>();
            for (int i = 0; i < 10; i++)
            {
                var customer = new CustomerProject
                {
                    Id = new Java.Lang.Integer(i),
                    CustomerId = new Java.Lang.Integer(i),
                    Name = "Customer " + i
                };
                customer.SetProject(new Java.Lang.Boolean(false));
                customers.Add(customer);
                if (i % 3 == 0)
                {
                    var project = new CustomerProject();
                    project.SetProject(new Java.Lang.Boolean(true));
                    project.CustomerId = customer.Id;
                    project.Id = new Java.Lang.Integer(i * 100);
                    project.Name = customer.Name + " : " + "project " + project.Id;
                    customers.Add(project);
                }
            }
            VeryfiLens.Settings.Customers = customers;
        }

        private void SetCategories()
        {
            List<Category> categories = new List<Category>();
            for (int i = 0; i < 10; i++)
            {
                Category category = new Category
                {
                    Id = new Java.Lang.Integer(i),
                    Name = "Category Number " + i,
                    Type = "mo_expense"
                };
                categories.Add(category);
            }
            VeryfiLens.Settings.CategoriesList = categories;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            VeryfiLens.ShowCamera();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private void ShowLogs(string log)
        {
            var tv_logs = FindViewById<Android.Widget.TextView>(Resource.Id.tv_logs);
            var text = tv_logs.Text + log + "\n";
            tv_logs.Text = text;
        }

        private class VeryfiLensDelegateListener : Java.Lang.Object, IVeryfiLensDelegate
        {
            private readonly MainActivity mainActivity;

            public VeryfiLensDelegateListener(MainActivity mainActivity)
            {
                this.mainActivity = mainActivity;
            }

            void IVeryfiLensDelegate.VeryfiLensClose(JSONObject json)
            {
                mainActivity.ShowLogs(json.ToString());
            }

            void IVeryfiLensDelegate.VeryfiLensError(JSONObject json)
            {
                mainActivity.ShowLogs(json.ToString());
            }

            void IVeryfiLensDelegate.VeryfiLensSuccess(JSONObject json)
            {
                mainActivity.ShowLogs(json.ToString());
            }

            void IVeryfiLensDelegate.VeryfiLensUpdate(JSONObject json)
            {
                mainActivity.ShowLogs(json.ToString());
            }
        }
    }
}
