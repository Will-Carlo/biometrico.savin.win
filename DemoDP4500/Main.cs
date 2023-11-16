using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DemoDP4500
{
    public partial class Main : CaptureForm
    {
        String superVar = "verify"; // verify register
        // PARA LAS VERIFICACIONES
        private DPFP.Template Template;
        private DPFP.Verification.Verification Verificator;
        // PARA LOS REGISTROS
        public delegate void OnTemplateEventHandler(DPFP.Template template);
        public event OnTemplateEventHandler OnTemplate;
        private DPFP.Processing.Enrollment Enroller;
        public Main()
        {
            InitializeComponent();
        }
  

        public void Verify(DPFP.Template template)
        {
            Template = template;
            ShowDialog();
        }

        protected override void Init()
        {
            if (superVar== "verify")
            {
                base.Init();
                base.Text = "Verificación de Huella Digital";
                Verificator = new DPFP.Verification.Verification();     // Create a fingerprint template verificator
                UpdateStatus(0);
            }
            else
            {
                base.Init();
                base.Text = "Dar de alta Huella";
                Enroller = new DPFP.Processing.Enrollment();            // Create an enrollment.
                UpdateStatus(0); // adding 0
            }
        }

        private void UpdateStatus(int FAR)
        {
            // Show "False accept rate" value
            SetStatus(String.Format("False Accept Rate (FAR) = {0}", FAR));
        }

        protected override void Process(DPFP.Sample Sample)
        {
            base.Process(Sample);
            if (superVar == "verify")
            {

                // Process the sample and create a feature set for the enrollment purpose.
                DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Verification);

                // Check quality of the sample and start verification if it's good
                // TODO: move to a separate task
                if (features != null)
                {
                    // Compare the feature set with our template
                    DPFP.Verification.Verification.Result result = new DPFP.Verification.Verification.Result();

                    DPFP.Template template = new DPFP.Template();
                    Stream stream;

                    // Extraer la bd en json de los empleados del local, comparar y responder al servidor con el if
                    //foreach (var emp in contexto.Empleadoes)
                    //{
                    //    stream = new MemoryStream(emp.Huella);
                    //    template = new DPFP.Template(stream);

                    //    Verificator.Verify(features, template, ref result);
                    //    UpdateStatus(result.FARAchieved);
                    //    if (result.Verified)
                    //    {
                    //        MakeReport("The fingerprint was VERIFIED. " + emp.Nombre);
                    //        break;
                    //    }
                    //}
                }
            }
            else
            {
                // Process the sample and create a feature set for the enrollment purpose.
                DPFP.FeatureSet features = ExtractFeatures(Sample, DPFP.Processing.DataPurpose.Enrollment);

                // Check quality of the sample and add to enroller if it's good
                if (features != null) try
                    {
                        MakeReport("The fingerprint feature set was created1.");
                        Enroller.AddFeatures(features);     // Add feature set to template.
                    }
                    finally
                    {
                        UpdateStatus();

                        // Check if template has been created.
                        switch (Enroller.TemplateStatus)
                        {
                            case DPFP.Processing.Enrollment.Status.Ready:   // report success and stop capturing
                                OnTemplate(Enroller.Template);
                                SetPrompt("Click Close, and then click Fingerprint Verification.");
                                Stop();
                                break;

                            case DPFP.Processing.Enrollment.Status.Failed:  // report failure and restart capturing
                                Enroller.Clear();
                                Stop();
                                UpdateStatus();
                                OnTemplate(null);
                                Start();
                                break;
                        }
                    }
            }
        }
        private void UpdateStatus()
        {
            // Show number of samples needed.
            SetStatus(String.Format("Fingerprint samples needed: {0}", Enroller.FeaturesNeeded));
        }




    }
}
