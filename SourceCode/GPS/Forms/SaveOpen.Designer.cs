﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace AgOpenGPS
{
    
    public partial class  FormGPS
    {
        //list of the list of patch data individual triangles for field sections
        public List<List<vec2>> patchSaveList = new List<List<vec2>>();

        //list of the list of patch data individual triangles for contour tracking
        public List<List<vec4>> contourSaveList = new List<List<vec4>>();

        //function that save vehicle and section settings
        public void FileSaveVehicle()
        {
            //in the current application directory
            //string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //string fieldDir = dir + "\\fields\\";

            string dirVehicle = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AgOpenGPS\\Vehicles\\";

            string directoryName = Path.GetDirectoryName(dirVehicle);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.Title = "Save Vehicle";
            saveDialog.Filter = "Text Files (*.txt)|*.txt";
            saveDialog.InitialDirectory = directoryName;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                vehiclefileName = Path.GetFileNameWithoutExtension(saveDialog.FileName) + " - ";
                Properties.Settings.Default.setVehicle_Name = vehiclefileName;

                using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                {
                    writer.WriteLine("Version," + Application.ProductVersion);
                    writer.WriteLine("Overlap," + Properties.Settings.Default.setVehicle_toolOverlap);
                    writer.WriteLine("ToolTrailingHitchLength," + Properties.Settings.Default.setVehicle_toolTrailingHitchLength);
                    writer.WriteLine("TankTrailingHitchLength," + Properties.Settings.Default.setVehicle_tankTrailingHitchLength);
                    writer.WriteLine("AntennaHeight," + Properties.Settings.Default.setVehicle_antennaHeight);
                    writer.WriteLine("LookAhead," + Properties.Settings.Default.setVehicle_lookAhead);
                    writer.WriteLine("AntennaPivot," + Properties.Settings.Default.setVehicle_antennaPivot);

                    writer.WriteLine("HitchLength," + Properties.Settings.Default.setVehicle_hitchLength);
                    writer.WriteLine("ToolOffset," + Properties.Settings.Default.setVehicle_toolOffset);
                    writer.WriteLine("TurnOffDelay," + Properties.Settings.Default.setVehicle_turnOffDelay);
                    writer.WriteLine("Wheelbase," + Properties.Settings.Default.setVehicle_wheelbase);

                    writer.WriteLine("IsPivotBehindAntenna," + Properties.Settings.Default.setVehicle_isPivotBehindAntenna);
                    writer.WriteLine("IsSteerAxleAhead," + Properties.Settings.Default.setVehicle_isSteerAxleAhead); 
                    writer.WriteLine("IsToolBehindPivot," + Properties.Settings.Default.setVehicle_isToolBehindPivot);
                    writer.WriteLine("IsToolTrailing," + Properties.Settings.Default.setVehicle_isToolTrailing);

                    writer.WriteLine("Spinner1," + Properties.Settings.Default.setSection_position1);
                    writer.WriteLine("Spinner2," + Properties.Settings.Default.setSection_position2);
                    writer.WriteLine("Spinner3," + Properties.Settings.Default.setSection_position3);
                    writer.WriteLine("Spinner4," + Properties.Settings.Default.setSection_position4);
                    writer.WriteLine("Spinner5," + Properties.Settings.Default.setSection_position5);
                    writer.WriteLine("Spinner6," + Properties.Settings.Default.setSection_position6);
                    writer.WriteLine("Spinner7," + Properties.Settings.Default.setSection_position7);
                    writer.WriteLine("Spinner8," + Properties.Settings.Default.setSection_position8);
                    writer.WriteLine("Spinner9," + Properties.Settings.Default.setSection_position9);

                    writer.WriteLine("Sections," + Properties.Settings.Default.setVehicle_numSections);
                    writer.WriteLine("ToolWidth," + Properties.Settings.Default.setVehicle_toolWidth);

                    writer.WriteLine("WorkSwitch," + Properties.Settings.Default.setIsWorkSwitchEnabled);
                    writer.WriteLine("ActiveLow," + Properties.Settings.Default.setIsWorkSwitchActiveLow);

                    writer.WriteLine("CamPitch," + Properties.Settings.Default.setCam_pitch);

                    writer.WriteLine("IsAtanCam," + Properties.Settings.Default.setCam_isAtanCam);
                    writer.WriteLine("TriangleResolution," + Properties.Settings.Default.setDisplay_triangleResolution);
                    writer.WriteLine("IsMetric," + Properties.Settings.Default.setIsMetric);
                    writer.WriteLine("IsGridOn," + Properties.Settings.Default.setIsGridOn);
                    writer.WriteLine("IsLightBarOn," + Properties.Settings.Default.setIsLightbarOn);
                    writer.WriteLine("IsAreaRight," + Properties.Settings.Default.setIsAreaRight);

                    writer.WriteLine("FieldColorR," + Properties.Settings.Default.setFieldColorR);
                    writer.WriteLine("FieldColorG," + Properties.Settings.Default.setFieldColorG);
                    writer.WriteLine("FieldColorB," + Properties.Settings.Default.setFieldColorB);
                    writer.WriteLine("SectionColorR," + Properties.Settings.Default.setSectionColorR);
                    writer.WriteLine("SectionColorG," + Properties.Settings.Default.setSectionColorG);
                    writer.WriteLine("SectionColorB," + Properties.Settings.Default.setSectionColorB);

                    writer.WriteLine("SlowSpeedCutoff," + Properties.Settings.Default.setVehicle_slowSpeedCutoff);

                    writer.WriteLine("IMUPitchZero," + Properties.Settings.Default.setIMU_pitchZero);
                    writer.WriteLine("IMURollZero," + Properties.Settings.Default.setIMU_rollZero);
                    writer.WriteLine("IsLogNMEA," + Properties.Settings.Default.setIsLogNMEA);
                    writer.WriteLine("MinFixStep," + Properties.Settings.Default.set_minFixStep);

                    writer.WriteLine("pidP," + Properties.Settings.Default.setAS_Kp);
                    writer.WriteLine("pidI," + Properties.Settings.Default.setAS_Ki);
                    writer.WriteLine("pidD," + Properties.Settings.Default.setAS_Kd);
                    writer.WriteLine("pidO," + Properties.Settings.Default.setAS_Ko);
                    writer.WriteLine("SteerAngleOffset," + Properties.Settings.Default.setAS_steerAngleOffset);

                    writer.WriteLine("minPWM," + Properties.Settings.Default.setAS_minSteerPWM);

                    writer.WriteLine("ToolMinUnappliedPixels," + Properties.Settings.Default.setVehicle_minApplied);

                }
                //little show to say saved and where

                var form = new FormTimedMessage(this, 3000, "Saved in Folder: ", dirVehicle);
                form.Show();

                Properties.Settings.Default.Save();
            }

        }

        //function to open a previously saved field
        public void FileOpenVehicle()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            //get the directory where the fields are stored
            string directoryName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AgOpenGPS\\Vehicles\\";

            //make sure the directory exists, if not, create it
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //the initial directory, fields, for the open dialog
            ofd.InitialDirectory = directoryName;

            //When leaving dialog put windows back where it was
            ofd.RestoreDirectory = true;

            //set the filter to text files only
            ofd.Filter = "txt files (*.txt)|*.txt";

            //was a file selected
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //if job started close it
                if (isJobStarted) JobClose();

                //make sure the file if fully valid and vehicle matches sections
                using (StreamReader reader = new StreamReader(ofd.FileName))
                {
                    try
                    {
                        string line;
                        Properties.Settings.Default.setVehicle_Name = ofd.FileName;
                        string[] words;
                        line = reader.ReadLine(); words = line.Split(',');

                        if (words[0] != "Version")

                        {
                            var form = new FormTimedMessage(this, 5000, "Vehicle File is Wrong Version", "Must be Version 2.14 or higher");
                            form.Show();
                            return;
                        }

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_toolOverlap = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_toolTrailingHitchLength = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_tankTrailingHitchLength = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_antennaHeight = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_lookAhead = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_antennaPivot = double.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_hitchLength = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_toolOffset = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_turnOffDelay = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_wheelbase = double.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_isPivotBehindAntenna = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_isSteerAxleAhead = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_isToolBehindPivot = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_isToolTrailing = bool.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSection_position1 = decimal.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSection_position2 = decimal.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSection_position3 = decimal.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSection_position4 = decimal.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSection_position5 = decimal.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSection_position6 = decimal.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSection_position7 = decimal.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSection_position8 = decimal.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSection_position9 = decimal.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_numSections = int.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_toolWidth = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIsWorkSwitchEnabled = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIsWorkSwitchActiveLow = bool.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setCam_pitch = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setCam_isAtanCam = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setDisplay_triangleResolution = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIsMetric = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIsGridOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIsLightbarOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIsAreaRight = bool.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setFieldColorR = byte.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setFieldColorG = byte.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setFieldColorB = byte.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSectionColorR = byte.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSectionColorG = byte.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setSectionColorB = byte.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setVehicle_slowSpeedCutoff = double.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIMU_pitchZero = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIMU_rollZero = double.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setIsLogNMEA = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.set_minFixStep = double.Parse(words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setAS_Kp = byte.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setAS_Ki = byte.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setAS_Kd = byte.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setAS_Ko = byte.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setAS_steerAngleOffset = byte.Parse(words[1]);

                        //**********new versions of load/save vehicle must test for older vehicle files****************

                        //set a default value if not exist in file, next save they will be there
                        mc.autoSteerData[mc.ssSteerOffset] = Properties.Settings.Default.setAS_steerAngleOffset;

                        //read in new values only if they exist
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setAS_steerAngleOffset = byte.Parse(words[1]);
                            mc.autoSteerData[mc.ssSteerOffset] = Properties.Settings.Default.setAS_steerAngleOffset;

                            line = reader.ReadLine(); words = line.Split(',');

                        }

                        //set a default value if not exist in file, next save they will be there
                        vehicle.toolMinUnappliedPixels = Properties.Settings.Default.setVehicle_minApplied;

                        //read in new values only if they exist
                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setVehicle_minApplied = int.Parse(words[1]);
                            vehicle.toolMinUnappliedPixels = Properties.Settings.Default.setVehicle_minApplied;

                            line = reader.ReadLine(); words = line.Split(',');

                        }

                        //Properties.Settings.Default.setPort_portNameGPS = (words[1]);
                        //Properties.Settings.Default.setPort_baudRate = int.Parse(words[1]);
                        //Properties.Settings.Default.setPort_portNameRelay = (words[1]);
                        //Properties.Settings.Default.setPort_wasRelayConnected = bool.Parse(words[1]);
                        //Properties.Settings.Default.setPort_portNameAutoSteer = (words[1]);
                        //Properties.Settings.Default.setPort_wasAutoSteerConnected = bool.Parse(words[1]);

                        vehiclefileName = Path.GetFileNameWithoutExtension(ofd.FileName) + " - ";
                        Properties.Settings.Default.setVehicle_Name = vehiclefileName;

                        Properties.Settings.Default.Save();

                        //get the number of sections from settings
                        vehicle.numOfSections = Properties.Settings.Default.setVehicle_numSections;
                        vehicle.numSuperSection = vehicle.numOfSections + 1;

                        //from settings grab the vehicle specifics
                        vehicle.toolOverlap = Properties.Settings.Default.setVehicle_toolOverlap;
                        vehicle.toolTrailingHitchLength = Properties.Settings.Default.setVehicle_toolTrailingHitchLength;
                        vehicle.tankTrailingHitchLength = Properties.Settings.Default.setVehicle_tankTrailingHitchLength;
                        vehicle.antennaHeight = Properties.Settings.Default.setVehicle_antennaHeight;
                        vehicle.toolLookAhead = Properties.Settings.Default.setVehicle_lookAhead;

                        vehicle.antennaPivot = Properties.Settings.Default.setVehicle_antennaPivot;
                        vehicle.hitchLength = Properties.Settings.Default.setVehicle_hitchLength;
                        vehicle.toolOffset = Properties.Settings.Default.setVehicle_toolOffset;
                        vehicle.toolTurnOffDelay = Properties.Settings.Default.setVehicle_turnOffDelay;
                        vehicle.wheelbase = Properties.Settings.Default.setVehicle_wheelbase;

                        vehicle.isToolTrailing = Properties.Settings.Default.setVehicle_isToolTrailing;
                        vehicle.isPivotBehindAntenna = Properties.Settings.Default.setVehicle_isPivotBehindAntenna;
                        vehicle.isSteerAxleAhead = Properties.Settings.Default.setVehicle_isSteerAxleAhead;
                        vehicle.isPivotBehindAntenna = Properties.Settings.Default.setVehicle_isToolBehindPivot;

                        mc.autoSteerSettings[mc.ssKp] = Properties.Settings.Default.setAS_Kp;
                        mc.autoSteerSettings[mc.ssKi] = Properties.Settings.Default.setAS_Ki;
                        mc.autoSteerSettings[mc.ssKd] = Properties.Settings.Default.setAS_Kd;
                        mc.autoSteerSettings[mc.ssKo] = Properties.Settings.Default.setAS_Ko;
                        mc.autoSteerSettings[mc.ssSteerOffset] = Properties.Settings.Default.setAS_steerAngleOffset;

                        mc.isWorkSwitchEnabled = Properties.Settings.Default.setIsWorkSwitchEnabled;
                        mc.isWorkSwitchActiveLow = Properties.Settings.Default.setIsWorkSwitchActiveLow;

                        //Set width of section and positions for each section
                        SectionSetPosition();

                        //Calculate total width and each section width
                        SectionCalcWidths();

                        //enable disable manual buttons
                        LineUpManualBtns();

                        btnSection1Man.Enabled = false;
                        btnSection2Man.Enabled = false;
                        btnSection3Man.Enabled = false;
                        btnSection4Man.Enabled = false;
                        btnSection5Man.Enabled = false;
                        btnSection6Man.Enabled = false;
                        btnSection7Man.Enabled = false;
                        btnSection8Man.Enabled = false;

                        camera.camPitch = Properties.Settings.Default.setCam_pitch;

                        isAtanCam = Properties.Settings.Default.setCam_isAtanCam;
                        triangleResolution = Properties.Settings.Default.setDisplay_triangleResolution;

                        isMetric = Properties.Settings.Default.setIsMetric;
                        if (isMetric) metricToolStrip.Checked = true; else metricToolStrip.Checked = false;
                        if (isMetric) imperialToolStrip.Checked = false; else imperialToolStrip.Checked = true;

                        isGridOn = Properties.Settings.Default.setIsGridOn;
                        if (isGridOn) gridToolStripMenuItem.Checked = true;
                        else gridToolStripMenuItem.Checked = false;

                        isLightbarOn = Properties.Settings.Default.setIsLightbarOn;
                        if (isLightbarOn) lightbarToolStripMenuItem.Checked = true;
                        else lightbarToolStripMenuItem.Checked = false;

                        isAreaOnRight = Properties.Settings.Default.setIsAreaRight;

                        redSections = Properties.Settings.Default.setSectionColorR;
                        grnSections = Properties.Settings.Default.setSectionColorG;
                        bluSections = Properties.Settings.Default.setSectionColorB;
                        redField = Properties.Settings.Default.setFieldColorR;
                        grnField = Properties.Settings.Default.setFieldColorG;
                        bluField = Properties.Settings.Default.setFieldColorB;

                        vehicle.slowSpeedCutoff = Properties.Settings.Default.setVehicle_slowSpeedCutoff;

                        pitchZero = Properties.Settings.Default.setIMU_pitchZero;
                        rollZero = Properties.Settings.Default.setIMU_rollZero;
                        isLogNMEA = Properties.Settings.Default.setIsLogNMEA;
                        minFixStepDist = Properties.Settings.Default.set_minFixStep;

                        //Application.Exit();
                    }
                    catch (Exception e) //FormatException e || IndexOutOfRangeException e2)
                    {
                        WriteErrorLog("Open Vehicle" + e.ToString());

                        //vehicle is corrupt, reload with all default information
                        Properties.Settings.Default.Reset();
                        Properties.Settings.Default.Save();
                        MessageBox.Show("Program will Reset to Recover. Please Restart", "Vehicle file is Corrupt", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Application.Exit();
                    }
                }
            }      //cancelled out of open file
        }//end of open file

        //function to open a previously saved field, Contour, Flags, Boundary
        public void FileOpenField(string _openType)
        {
            string fileAndDirectory;
            //get the directory where the fields are stored
            //string directoryName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\fields\\";

            if (_openType == "Resume")
            {
                //Either exit or update running save
                fileAndDirectory = workingDirectory + currentFieldDirectory + "\\Field.txt";
                if (!File.Exists(fileAndDirectory)) return;
            }

            //open file dialog instead
            else
            {
                //create the dialog instance
                OpenFileDialog ofd = new OpenFileDialog();

                //the initial directory, fields, for the open dialog
                ofd.InitialDirectory = workingDirectory;

                //When leaving dialog put windows back where it was
                ofd.RestoreDirectory = true;

                //set the filter to text files only
                ofd.Filter = "Field files (Field.txt)|Field.txt";

                //was a file selected
                if (ofd.ShowDialog() == DialogResult.Cancel) return;
                else fileAndDirectory = ofd.FileName;
            }

            //close the existing job and reset everything
            this.JobClose();

            //and open a new job
            this.JobNew();

            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            //start to read the file
            string line;
            using (StreamReader reader = new StreamReader(fileAndDirectory))
            {
                try
                {
                    //Date time line
                    line = reader.ReadLine();

                    //dir header $FieldDir
                    line = reader.ReadLine();

                    //read field directory
                    line = reader.ReadLine();

                    currentFieldDirectory = line.Trim();

                    //Offset header
                    line = reader.ReadLine();

                    //read the Offsets 
                    line = reader.ReadLine();
                    string[] offs = line.Split(',');
                    pn.utmEast = int.Parse(offs[0]);
                    pn.utmNorth = int.Parse(offs[1]);
                    pn.zone = int.Parse(offs[2]);

                    //create a new grid
                    worldGrid.CreateWorldGrid(pn.actualNorthing - pn.utmNorth, pn.actualEasting - pn.utmEast);

                    //read the section and patch triangles...

                    //read the line $Sections
                    line = reader.ReadLine();

                    //read number of sections
                    line = reader.ReadLine();

                    totalSquareMeters = 0;
                    //totalUserSquareMeters = 0;
                    userDistance = 0;

                    vec2 vecFix = new vec2(0, 0);

                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        int verts = int.Parse(line);

                        section[0].triangleList = new List<vec2>();
                        section[0].patchList.Add(section[0].triangleList);

                        for (int v = 0; v < verts; v++)
                        {
                            line = reader.ReadLine();
                            string[] words = line.Split(',');
                            vecFix.easting = double.Parse(words[0]);
                            vecFix.northing = double.Parse(words[1]);
                            section[0].triangleList.Add(vecFix);
                        }

                        //calculate area of this patch - AbsoluteValue of (Ax(By-Cy) + Bx(Cy-Ay) + Cx(Ay-By)/2)
                        verts -= 2;
                        if (verts >= 2)
                        {
                            for (int j = 0; j < verts; j++)
                            {
                                double temp = 0;
                                temp = section[0].triangleList[j].easting * (section[0].triangleList[j + 1].northing - section[0].triangleList[j + 2].northing) +
                                          section[0].triangleList[j + 1].easting * (section[0].triangleList[j + 2].northing - section[0].triangleList[j].northing) +
                                              section[0].triangleList[j + 2].easting * (section[0].triangleList[j].northing - section[0].triangleList[j + 1].northing);

                                totalSquareMeters += Math.Abs((temp / 2.0));
                            }
                        }
                    }
                }

                catch (Exception e)
                {
                    WriteErrorLog("While Opening Field" + e.ToString());

                    var form = new FormTimedMessage(this, 4000, "Field File is Corrupt", "Choose a different field");
                    form.Show();
                    JobClose();
                    return;
                }
            }

            // Contour points ----------------------------------------------------------------------------

            fileAndDirectory = workingDirectory + currentFieldDirectory + "\\Contour.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(this, 4000, "Missing Contour File", "But Field is Loaded");
                form.Show();
                //return;
            }

            /*
                May-14-17  -->  7:42:47 PM
                Points in Patch followed by easting, heading, northing, altitude
                $ContourDir
                cdert_May14
                $Offsets
                533631,5927279,12
                19
                2.866,2.575,-4.07,0             
             */
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read the lines and skip them
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            //read how many vertices in the following patch
                            line = reader.ReadLine();
                            int verts = int.Parse(line);

                            vec4 vecFix = new vec4(0, 0, 0, 0);

                            ct.ptList = new List<vec4>();
                            ct.stripList.Add(ct.ptList);

                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                vecFix.x = double.Parse(words[0]);
                                vecFix.y = double.Parse(words[1]);
                                vecFix.z = double.Parse(words[2]);
                                vecFix.k = double.Parse(words[3]);

                                ct.ptList.Add(vecFix);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        WriteErrorLog("Loading Contour file" + e.ToString());

                        var form = new FormTimedMessage(this, 4000, "Contour File is Corrupt", "But Field is Loaded");
                        form.Show();
                    }
                }
            }


            // Flags -------------------------------------------------------------------------------------------------

            //Either exit or update running save
            fileAndDirectory = workingDirectory + currentFieldDirectory + "\\Flags.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(this, 4000, "Missing Flags File", "But Field is Loaded");
                form.Show();
            }

            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read the lines and skip them
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine();
                        int points = int.Parse(line);

                        if (points > 0)
                        {
                            double lat;
                            double longi;
                            double east;
                            double nort;
                            int color, ID;

                            for (int v = 0; v < points; v++)
                            {

                                line = reader.ReadLine();
                                string[] words = line.Split(',');

                                lat = double.Parse(words[0]);
                                longi = double.Parse(words[1]);
                                east = double.Parse(words[2]);
                                nort = double.Parse(words[3]);
                                color = int.Parse(words[4]);
                                ID = int.Parse(words[5]);

                                CFlag flagPt = new CFlag(lat, longi, east, nort, color, ID);
                                flagPts.Add(flagPt);
                            }

                        }
                    }

                    catch (Exception e)
                    {
                        var form = new FormTimedMessage(this, 4000, "Flag File is Corrupt", "But Field is Loaded");
                        form.Show();
                        WriteErrorLog("FieldOpen, Loading Flags, Corrupt Flag File" + e.ToString());
                    }
                }
            }


            // ABLine -------------------------------------------------------------------------------------------------

            //Either exit or update running save
            fileAndDirectory = workingDirectory + currentFieldDirectory + "\\ABLine.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(this, 4000, "Missing ABLine File", "But Field is Loaded");
                form.Show();
            }

            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read the lines and skip them
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine();
                        bool isAB = bool.Parse(line);

                        if (isAB)
                        {
                            //set gui image button on
                            btnABLine.Image = global::AgOpenGPS.Properties.Resources.ABLineOn;

                            //Heading  , ,refPoint2x,z                    
                            line = reader.ReadLine();
                            ABLine.abHeading = double.Parse(line);

                            //refPoint1x,z
                            line = reader.ReadLine();
                            string[] words = line.Split(',');
                            ABLine.refPoint1.easting = double.Parse(words[0]);
                            ABLine.refPoint1.northing = double.Parse(words[1]);

                            //refPoint2x,z
                            line = reader.ReadLine();
                            words = line.Split(',');
                            ABLine.refPoint2.easting = double.Parse(words[0]);
                            ABLine.refPoint2.northing = double.Parse(words[1]);

                            //Tramline
                            line = reader.ReadLine();
                            words = line.Split(',');
                            ABLine.tramPassEvery = int.Parse(words[0]);
                            ABLine.passBasedOn = int.Parse(words[1]);

                            ABLine.refABLineP1.easting = ABLine.refPoint1.easting - Math.Sin(ABLine.abHeading) * 10000.0;
                            ABLine.refABLineP1.northing = ABLine.refPoint1.northing - Math.Cos(ABLine.abHeading) * 10000.0;

                            ABLine.refABLineP2.easting = ABLine.refPoint1.easting + Math.Sin(ABLine.abHeading) * 10000.0;
                            ABLine.refABLineP2.northing = ABLine.refPoint1.northing + Math.Cos(ABLine.abHeading) * 10000.0;

                            ABLine.isABLineSet = true;
                        }
                    }

                    catch (Exception e)
                    {
                        var form = new FormTimedMessage(this, 4000, "AB Line File is Corrupt", "But Field is Loaded");
                        form.Show();
                        WriteErrorLog("Load AB Line" + e.ToString());

                    }
                }


                // Boundary  -------------------------------------------------------------------------------------------------

                //Either exit or update running save
                fileAndDirectory = workingDirectory + currentFieldDirectory + "\\Boundary.txt";
                if (!File.Exists(fileAndDirectory))
                {
                    var form = new FormTimedMessage(this, 4000, "Missing Boundary File", "But Field is Loaded");
                    form.Show();
                }

                //2017-June-05 09:48:52 PM
                //Points in line followed by easting, northing
                //$BoundaryDir
                //gtr Jun05
                //$Offsets
                //555799,5921092,12
                //$NumLinePoints
                //27
                //-3.1814080592639,-3.1814080592639
                //-3.16491526875197,-3.16491526875197

                else
                {
                    using (StreamReader reader = new StreamReader(fileAndDirectory))
                    {
                        try
                        {
                            //read the lines and skip them
                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            line = reader.ReadLine();

                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);

                            if (numPoints > 0)
                            {
                                vec2 vecPt = new vec2(0, 0);
                                boundary.ptList.Clear();

                                //load the line
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vecPt.easting = double.Parse(words[0]);
                                    vecPt.northing = double.Parse(words[1]);
                                    boundary.ptList.Add(vecPt);
                                }

                                boundary.CalculateBoundaryArea();
                                boundary.PreCalcBoundaryLines();
                                if (boundary.area > 0) boundary.isSet = true;
                                else boundary.isSet = false;
                            }
                        }

                        catch (Exception e)
                        {
                            var form = new FormTimedMessage(this, 4000, "Boundary Line File is Corrupt", "But Field is Loaded");
                            form.Show();
                            WriteErrorLog("Load Boundary Line" + e.ToString());

                        }
                    }


                }
            }

        }//end of open file

        //creates the field file when starting new field
        public void FileCreateField()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            if (!isJobStarted)
            {
                using (var form = new FormTimedMessage(this, 3000, "Ooops, Job Not Started", "Start a Job First"))
                { form.Show(); }
                return;
            }
            string myFileName, dirField;

            //get the directory and make sure it exists, create if not
            dirField = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                                                                    "\\AgOpenGPS\\Fields\\" + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Field.txt";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt"));

                writer.WriteLine("$FieldDir");
                writer.WriteLine(currentFieldDirectory);

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine(pn.utmEast + "," + pn.utmNorth + "," + pn.zone);

                //write paths # of sections
                writer.WriteLine("$Sections");
                writer.WriteLine(vehicle.numOfSections + 1);
            }

        }

        //save field Patches
        public void FileSaveField()
        {
            //make sure there is something to save
            if (patchSaveList.Count() > 0)
            {
                //Append the current list to the field file
                using (StreamWriter writer = new StreamWriter((workingDirectory + currentFieldDirectory + "\\Field.txt"), true))
                {
                    //for each patch, write out the list of triangles to the file
                    foreach (var triList in patchSaveList)
                    {
                        int count2 = triList.Count();
                        writer.WriteLine(count2);

                        for (int i = 0; i < count2; i++)
                            writer.WriteLine(triList[i].easting + "," + triList[i].northing);
                    }
                }

                //clear out that patchList and begin adding new ones for next save
                patchSaveList.Clear();
            }
        }

        //Create contour file
        public void FileCreateContour()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //12  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude
            //$ContourDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12

            //get the directory and make sure it exists, create if not
            string dirField = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                         "\\AgOpenGPS\\Fields\\" + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Contour.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt"));
                writer.WriteLine("Points in Patch followed by easting, heading, northing, altitude");

                //which field directory
                writer.WriteLine("$ContourDir");
                writer.WriteLine(currentFieldDirectory);

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine(pn.utmEast + "," + pn.utmNorth + "," + pn.zone);
            }
        }

        //save the contour points which include elevation values
        public void FileSaveContour()
        {
            //1  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //make sure there is something to save
            if (contourSaveList.Count() > 0)
            {
                //Append the current list to the field file
                using (StreamWriter writer = new StreamWriter((workingDirectory + currentFieldDirectory + "\\Contour.txt"), true))
                {

                    //for every new chunk of patch in the whole section
                    foreach (var triList in contourSaveList)
                    {
                        int count2 = triList.Count;

                        writer.WriteLine(count2);

                        for (int i = 0; i < count2; i++)
                        {
                            writer.WriteLine(Math.Round(triList[i].x, 3) + "," +
                                Math.Round(triList[i].y, 3) + "," +
                                Math.Round(triList[i].z, 3) + "," +
                                Math.Round(triList[i].k, 3));
                        }
                    }
                }

                contourSaveList.Clear();

            }

            //set saving flag off
            isSavingFile = false;
        }

        //save the boundary
        public void FileSaveOuterBoundary()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //12  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude
            //$ContourDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12

            //get the directory and make sure it exists, create if not
            string dirField = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                         "\\AgOpenGPS\\Fields\\" + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + "boundary.Txt"))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt"));
                writer.WriteLine("Points in line followed by easting, northing");

                //which field directory
                writer.WriteLine("$BoundaryDir");
                writer.WriteLine(currentFieldDirectory);

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine(pn.utmEast + "," + pn.utmNorth + "," + pn.zone);

                writer.WriteLine("$NumLinePoints");
                
                writer.WriteLine(boundary.ptList.Count);
                if (boundary.ptList.Count > 0)
                {
                    for (int j = 0; j < boundary.ptList.Count; j++)
                        writer.WriteLine(boundary.ptList[j].easting + "," + boundary.ptList[j].northing);
                }
            }
        }

        //save all the flag markers
        public void FileSaveFlags()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FlagsDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //get the directory and make sure it exists, create if not
            string dirField = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                         "\\AgOpenGPS\\Fields\\" + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //use Streamwriter to create and overwrite existing flag file
            using (StreamWriter writer = new StreamWriter(dirField + "Flags.txt"))
            {
                try
                {
                    //Write out the date time
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt"));

                    //which field directory
                    writer.WriteLine("$FlagsDir");
                    writer.WriteLine(currentFieldDirectory);

                    //write out the easting and northing Offsets
                    writer.WriteLine("$Offsets");
                    writer.WriteLine(pn.utmEast + "," + pn.utmNorth + "," + pn.zone);

                    int count2 = flagPts.Count;

                    writer.WriteLine(count2);

                    for (int i = 0; i < count2; i++)
                    {
                        writer.WriteLine(
                            flagPts[i].latitude + "," +
                            flagPts[i].longitude + "," +
                            flagPts[i].easting + "," +
                            flagPts[i].northing + "," +
                            flagPts[i].color + "," +
                            flagPts[i].ID);
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n Cannot write to file.");
                    WriteErrorLog("Saving Flags" + e.ToString());

                    return;
                }

            }
        }

        //save all the flag markers
        public void FileSaveABLine()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM

            //get the directory and make sure it exists, create if not
            string dirField = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                         "\\AgOpenGPS\\Fields\\" + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //use Streamwriter to create and overwrite existing ABLine file
            using (StreamWriter writer = new StreamWriter(dirField + "ABLine.txt"))
            {
                try
                {
                    //Write out the date time
                    writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt"));

                    //write out the ABLine
                    writer.WriteLine("$Heading");

                    //true or false if ABLine is set
                    if (ABLine.isABLineSet) writer.WriteLine(true);
                    else writer.WriteLine(false);

                    writer.WriteLine(ABLine.abHeading);
                    writer.WriteLine(ABLine.refPoint1.easting + "," + ABLine.refPoint1.northing);
                    writer.WriteLine(ABLine.refPoint2.easting + "," + ABLine.refPoint2.northing);
                    writer.WriteLine(ABLine.tramPassEvery + "," + ABLine.passBasedOn);
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n Cannot write to file.");
                    WriteErrorLog("Saving AB Line" + e.ToString());

                    return;
                }

            }
        }

        //save nmea sentences
        public void FileSaveNMEA()
        {
            using (StreamWriter writer =  new StreamWriter((workingDirectory + currentFieldDirectory + "\\NMEA_log.txt"), true))
            {
                writer.Write(pn.logNMEASentence.ToString());
            }
            pn.logNMEASentence.Clear();
        }

        //generate KML file from flags
        public void FileSaveFlagsKML()
        {

            //get the directory and make sure it exists, create if not
            string dirField = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                         "\\AgOpenGPS\\Fields\\" + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flags.kml";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                for (int i = 0; i < count2; i++)
                {
                    writer.WriteLine(@"  <Placemark>                                  ");
                    writer.WriteLine(@"<Style> <IconStyle>");
                    if (flagPts[i].color == 0)  //red - xbgr
                        writer.WriteLine(@"<color>ff4400ff</color>");
                    if (flagPts[i].color == 1)  //grn - xbgr
                        writer.WriteLine(@"<color>ff44ff00</color>");
                    if (flagPts[i].color == 2)  //yel - xbgr
                        writer.WriteLine(@"<color>ff44ffff</color>");

                    writer.WriteLine(@"</IconStyle> </Style>");
                    writer.WriteLine(@" <name> " + (i+1) + @"</name>");
                    writer.WriteLine(@"<Point><coordinates> " +
                                    flagPts[i].longitude + "," + flagPts[i].latitude + ",0" +
                                    @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                       
                }

                writer.WriteLine(@"</Document>");

                writer.WriteLine(@"</kml>                                         ");


            
            }

        }

        //generate KML file from flag
        public void FileSaveSingleFlagKML(int flagNumber)
        {

            //get the directory and make sure it exists, create if not
            string dirField = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                         "\\AgOpenGPS\\Fields\\" + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                    writer.WriteLine(@"  <Placemark>                                  ");
                    writer.WriteLine(@"<Style> <IconStyle>");
                    if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                        writer.WriteLine(@"<color>ff4400ff</color>");
                    if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                        writer.WriteLine(@"<color>ff44ff00</color>");
                    if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                        writer.WriteLine(@"<color>ff44ffff</color>");
                    writer.WriteLine(@"</IconStyle> </Style>");
                    writer.WriteLine(@" <name> " + flagNumber + @"</name>");
                    writer.WriteLine(@"<Point><coordinates> " +
                                    flagPts[flagNumber-1].longitude + "," + flagPts[flagNumber-1].latitude + ",0" +
                                    @"</coordinates> </Point> ");
                    writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }
    }
}

//binary saves etc, cold storage

//AB Line
/*
                //write out the ABLine
                writer.WriteLine("$Heading");

                //true or false if ABLine is set
                if (ABLine.isABLineSet) writer.WriteLine(true);
                else writer.WriteLine(false);

                writer.WriteLine(ABLine.abHeading + "," + ABLine.refPoint1.x + ","
                    + ABLine.refPoint1.z + "," + ABLine.refPoint2.x + "," + ABLine.refPoint2.z + "," + ABLine.tramPassEvery + "," + ABLine.passBasedOn);
*/

//Binary save field
/*
//Function to save a field
public void FileSaveField()
{
   //Friday, February 10, 2017  -->  2:58:24 PM
    //$FieldDir
    //test_2017Feb10_02.55.46.PM
    //$Offsets
    //533210,5927965
    //$Heading
    //False
    //0,0.2,0.2,0.3,0.3,0,0
    //$Sections
    //3
    //4
    //76
    //-10.791,-10.964

    if (!isJobStarted)
    {
        using (var form = new FormTimedMessage(this, 3000, "Ooops, Job Not Started", "Start a job"))
        { form.Show(); }
        return;
    }

    //the saved filename
    string myFileName = workingDirectory + currentFieldDirectory + "\\Field.fld";

    //set saving flag on to ensure rapid save, no gps update
    isSavingFile = true;

 
    //make the file
    using (BinaryWriter bw = new BinaryWriter(File.Open(myFileName, FileMode.Create, FileAccess.Write)))
    {
        //Write out the date
        bw.Write(DateTime.Now.ToLongDateString() + "  -->  " + DateTime.Now.ToLongTimeString()); //string
 
        bw.Write("$FieldDir");
        bw.Write(currentFieldDirectory); //string

        //write out the easting and northing Offsets
        bw.Write("$Offsets");
        bw.Write(pn.utmEast); //int
        bw.Write(pn.utmNorth); //int
        bw.Write(pn.zone); //double

        //write out the ABLine
        bw.Write("$Heading");

        //true or false if ABLine is set
        if (ABLine.isABLineSet) bw.Write(true); //bool
        else bw.Write(false);

        bw.Write(ABLine.abHeading);
        bw.Write(ABLine.refPoint1.x);   // double
        bw.Write(ABLine.refPoint1.z);   // double
        bw.Write(ABLine.refPoint2.x);   // double
        bw.Write(ABLine.refPoint2.z);   // double
        bw.Write(ABLine.tramPassEvery); //int
        bw.Write(ABLine.passBasedOn);   //int
                 
        //write paths # of sections
        bw.Write("$Sections");
        bw.Write(vehicle.numOfSections+1);

        for (int j = 0; j < vehicle.numOfSections+1; j++)
        {
            //total patches for each section
            int patchCount = section[j].patchList.Count;

            //Write out the patches number
            bw.Write(patchCount);

            if (patchCount > 0)
            {
                //for every new chunk of patch in the whole section
                for (int q = 0; q < patchCount; q++ ) //each (var triList in section[j].patchList)
                {
                    int count2 = section[j].patchList[q].Count();

                    bw.Write(count2);

                    for (int i = 0; i < count2; i++)
                    {
                        bw.Write(section[j].patchList[q][i].x);
                        bw.Write(section[j].patchList[q][i].z);
                    }
                }
            }

        }

        bw.Write("$TotalSqM");
        bw.Write(totalSquareMeters); //double                
        //bw.Close();

    }

    //set saving flag off
    isSavingFile = false;
}

 * */

//save binary version of contour points
/*
        public void FileSaveContourPooper()
        {
            //get the directory and make sure it exists, create if not
            string dirField = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) +
                         "\\AgOpenGPS\\Fields\\" + currentFieldDirectory + "\\" + "Contour.ctr";;

            //set saving flag on to ensure rapid save, no gps update
            isSavingFile = true;

            //every time the patch turns off and on is a new patch
            int stripCount = ct.stripList.Count;

                       //create the file
            using (BinaryWriter bw = new BinaryWriter(File.Open(dirField, FileMode.Create, FileAccess.Write)))
            {
                //$Contour
                //First_2017Feb10_09.58.51.AM
                //$Offsets
                //533210,5927965,12
                //$Patches
                //2
                //76
                //1.384,3.135,-86.304,0

                //writing into the file
                try
                {
                    //which field directory
                    bw.Write("$FieldDir");
                    bw.Write(currentFieldDirectory);

                    //write out the easting and northing Offsets
                    bw.Write("$Offsets");
                    bw.Write(pn.utmEast); //int
                    bw.Write(pn.utmNorth); //int
                    bw.Write(pn.zone); //double

                    //write out how many strips total
                    bw.Write("$Patches");
                    bw.Write(stripCount);

                    if (stripCount > 0)
                    {
                        //for every new chunk of patch in the whole section
                        for (int i = 0; i < stripCount; i++)
                        {
                            int count2 = ct.stripList[i].Count;
                            bw.Write(count2);

                            for (int j = 0; j < count2; j++)
                            {
                                bw.Write(Math.Round(ct.stripList[i][j].x, 3));
                                bw.Write(Math.Round(ct.stripList[i][j].y, 3));
                                bw.Write(Math.Round(ct.stripList[i][j].z, 3));
                                bw.Write(Math.Round(ct.stripList[i][j].k, 3));
                            }
                        }
                    }

                }

                catch (IOException e)
                {
                    MessageBox.Show(e.Message, "\n Cannot write to file.");
                    return;
                }
                //bw.Close();

            }

            isSavingFile = false;

        }
         */


//binary open field
       //function to open a previously saved field, Contour, Flags
/*
        public void FileOpenField(string _openType)
        {
            string fileAndDirectory;
            //get the directory where the fields are stored
            //string directoryName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\fields\\";

            if (_openType == "Resume")
            {
                //Either exit or update running save
                fileAndDirectory = workingDirectory + currentFieldDirectory + "\\Field.fld";
                if (!File.Exists(fileAndDirectory)) return;
            }

            //open file dialog instead
            else
            {
                //create the dialog instance
                OpenFileDialog ofd = new OpenFileDialog();

                //the initial directory, fields, for the open dialog
                ofd.InitialDirectory = workingDirectory;

                //When leaving dialog put windows back where it was
                ofd.RestoreDirectory = true;

                //set the filter to text files only
                ofd.Filter = "fld files (*.fld)|*.fld";

                //was a file selected
                if (ofd.ShowDialog() == DialogResult.Cancel) return;
                else fileAndDirectory = ofd.FileName;
            }


            //Friday, February 10, 2017  -->  2:58:24 PM
            //$FieldDir
            //test_2017Feb10_02.55.46.PM
            //$Offsets
            //533210,5927965,12
            //$Heading
            //False
            //0,0.2,0.2,0.3,0.3,0,0
            //$Sections
            //3
            //4
            //76
            //-10.791,-10.964

            //close the existing job and reset everything
            this.JobClose();

            //and open a new job
            this.JobNew();

            using (BinaryReader br = new BinaryReader(new FileStream(fileAndDirectory, FileMode.Open)))
            {
                string s = br.ReadString(); //Date/time

                s = br.ReadString();        //$FieldDir
                if (s.IndexOf("$FieldDir") == -1)
                { MessageBox.Show("Sections header is Corrupt"); JobClose(); return; }


                currentFieldDirectory = br.ReadString(); //read current directory
                currentFieldDirectory = currentFieldDirectory.Trim();

                //Offset header
                s = br.ReadString(); //$Offsets

                //read the Offsets 
                pn.utmEast = br.ReadInt32();
                pn.utmNorth = br.ReadInt32();
                pn.zone = br.ReadDouble();

                worldGrid.CreateWorldGrid(pn.actualNorthing - pn.utmNorth, pn.actualEasting - pn.utmEast);

                //AB Line header
                s = br.ReadString(); //$Heading


                //read the boolean if AB is set
                bool b = br.ReadBoolean();

                //If is true there is AB Line data
                if (b)
                {
                    //set gui image button on
                    btnABLine.Image = global::AgOpenGPS.Properties.Resources.ABLineOn;

                    //Heading, refPoint1x,z,refPoint2x,z                    
                    ABLine.abHeading        = br.ReadDouble();
                    ABLine.refPoint1.x      = br.ReadDouble();
                    ABLine.refPoint1.z      = br.ReadDouble();
                    ABLine.refPoint2.x      = br.ReadDouble();
                    ABLine.refPoint2.z      = br.ReadDouble();
                    ABLine.tramPassEvery    = br.ReadInt32();
                    ABLine.passBasedOn      = br.ReadInt32();

                    ABLine.refABLineP1.x = ABLine.refPoint1.x - Math.Sin(ABLine.abHeading) * 10000.0;
                    ABLine.refABLineP1.z = ABLine.refPoint1.z - Math.Cos(ABLine.abHeading) * 10000.0;

                    ABLine.refABLineP2.x = ABLine.refPoint1.x + Math.Sin(ABLine.abHeading) * 10000.0;
                    ABLine.refABLineP2.z = ABLine.refPoint1.z + Math.Cos(ABLine.abHeading) * 10000.0;

                    ABLine.isABLineSet = true;
                }
                  
                //false so just read and skip the heading line, reset btn image
                else 
                {
                    btnABLine.Image = global::AgOpenGPS.Properties.Resources.ABLineOff;
                    
                    br.ReadDouble();
                    br.ReadDouble();
                    br.ReadDouble();
                    br.ReadDouble();
                    br.ReadDouble();
                    br.ReadInt32();
                    br.ReadInt32();
                }

                //read the line $Sections
                s = br.ReadString();        //$Sections
                if (s.IndexOf("$Sections") == -1)
                { MessageBox.Show("Sections header is Corrupt"); JobClose(); return; }

                //read number of sections
                int numSects = br.ReadInt32() - 1;        //$Sections

                //make sure sections in file matches sections set in current vehicle
                if (vehicle.numOfSections != numSects)
                { MessageBox.Show("# of Sections doesn't match this field"); JobClose(); return; }


                //finally start loading triangles
                vec2 vecFix = new vec2(0, 0);

                for (int j = 0; j < vehicle.numOfSections+1; j++)
                {
                    //now read number of patches, then how many vertex's
                    int patches = br.ReadInt32();
                    for (int k = 0; k < patches; k++)
                    {
                        section[j].triangleList = new List<vec2>();
                        section[j].patchList.Add(section[j].triangleList);
                        int verts = br.ReadInt32();
                        for (int v = 0; v < verts; v++)
                        {
                            vecFix.x = br.ReadDouble();
                            vecFix.z = br.ReadDouble();
                            section[j].triangleList.Add(vecFix);
                        }
                    }
                }

                s = br.ReadString();
                if (s.IndexOf("$TotalSqM") == -1)
                { MessageBox.Show("Meters header is Corrupt"); JobClose(); return; }
                
                totalSquareMeters = br.ReadDouble();//total square meters
            }

            // Contour points ----------------------------------------------------------------------------

            fileAndDirectory = workingDirectory + currentFieldDirectory + "\\Contour.ctr";
            if (!File.Exists(fileAndDirectory))
            {
                MessageBox.Show("Missing Contour File");
                return;
            }

            
                $Contour
                First_2017Feb10_09.58.51.AM
                //$Offsets
                //533210,5927965,12
                $Patches
                2
                76
                1.384,3.135,-86.304,0 -easting, heading, northing, altitude
             
            using (BinaryReader br = new BinaryReader(new FileStream(fileAndDirectory, FileMode.Open)))
            {
                string s = br.ReadString(); //$Contour string
                s = br.ReadString();        //current directory string


                //Offset header
                s = br.ReadString(); //$Offsets

                //read the Offsets  - Just read and skip them
                br.ReadInt32();
                br.ReadInt32();
                br.ReadDouble();

                //$Patches line
                s = br.ReadString();
                if (s.IndexOf("$Patches") == -1)
                { MessageBox.Show("Contour File is Corrupt"); return; }

                vec4 vecFix = new vec4(0, 0, 0, 0);

                //now read number of patches, then how many vertex's
                int patches = br.ReadInt32();

                for (int k = 0; k < patches; k++)
                {
                    ct.ptList = new List<vec4>();
                    ct.stripList.Add(ct.ptList);

                    int verts = br.ReadInt32();

                    for (int v = 0; v < verts; v++)
                    {
                        vecFix.x = br.ReadDouble();
                        vecFix.y = br.ReadDouble();
                        vecFix.z = br.ReadDouble();
                        vecFix.k = br.ReadDouble();  

                        ct.ptList.Add(vecFix);
                    }

                }
            }


       // Flags -------------------------------------------------------------------------------------------------

            //Either exit or update running save
            fileAndDirectory = workingDirectory + currentFieldDirectory + "\\Flags.flg";
            if (!File.Exists(fileAndDirectory))
            {
                MessageBox.Show("Missing Flag File");
                return;
            }

            using (BinaryReader br = new BinaryReader(new FileStream(fileAndDirectory, FileMode.Open)))
            {
                string s = br.ReadString();
                s = br.ReadString();

                //Offset header
                s = br.ReadString(); //$Offsets

                //read the Offsets - Just read and skip them
                br.ReadInt32();
                br.ReadInt32();
                br.ReadDouble();
 
                //CFlag flagPt = new CFlag(0,0,0,0,0);
                int points = br.ReadInt32();
 
                if (points > 0)
                {
                    double lat  ;
                    double longi;
                    double east ;
                    double nort ;
                    int color, ID;

                    for (int v = 0; v < points; v++)
                    {
                        
                    
                        lat = br.ReadDouble();
                        longi = br.ReadDouble();
                        east = br.ReadDouble();
                        nort = br.ReadDouble();
                        color = br.ReadInt32();
                        ID = br.ReadInt32();
                        
                        CFlag flagPt = new CFlag(lat, longi, east, nort, color, ID);
                        flagPts.Add(flagPt);
                     }
                    
                }
            }
        }//end of open file
*/