using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NgoCommunity.Models;
using NgoCommunity.ViewModels.Ngo;
using System.IO;


namespace NgoCommunity.Controllers.Ngo
{
    public class NgoController : Controller
    {

        NgoDBEntities db = new NgoDBEntities();

        // GET: Ngo
        public ActionResult Index()
        {
            return View();
        }

        [HandleError]
        public ActionResult NgoRegistration()
        {

            try
            {

                NgoViewModel nvm = new NgoViewModel();
                List<NgoModel> obj = new List<NgoModel>();
                var list = db.NgoMasters.ToList();
                foreach (var x in list)
                {
                    NgoModel S = new NgoModel();
                    S.ngoId = x.ngoId;
                    S.name = x.name;
                    S.emailId = x.emailId;
                    S.password = x.password;
                
                    S.panNo = x.panNo;
                    S.acheivement = x.acheivement;
                    S.uploadPan = x.uploadPan;
                    S.parentOrg = x.parentOrg;
                    S.websiteUrl = x.websiteUrl;
                    S.createdOn = x.createdOn;
                    S.createdBy = x.createdBy;
                    S.updatedOn = x.updatedOn;
                    S.updatedBy = x.updatedBy;
                    S.deletedOn = x.deletedOn;
                    S.deletedBy = x.createdBy;
                    S.isActive = x.isActive;
                    S.isApproved = x.isApproved;
                    S.isDeleted = x.isDeleted;
                    S.addressId = x.addressId;
                    S.userId = x.userId;

                    //var tbladdline1 = db.AddressMasters.Find(x.addressId);
                    //S.addressLine1 = tbladdline1.addressLine1;

                    S.AddressMaster = db.AddressMasters.Find(S.addressId);

                    S.UserMaster = db.UserMasters.Find(S.userId);
                    //S.addressLine2 = tbladdline1.addressLine2;
                    //S.addressLine3 = tbladdline1.addressLine3;
                    obj.Add(S);

                }



               // nvm.listam = adobj;

                nvm.listnm = obj;
                nvm.Countries = PopulateCountryDropDown("CountryName", "CountryId");
                return View(nvm);

            }
            catch (Exception)
            {
                 
                throw;
            }
            
        }

        [HttpPost]
        [HandleError]
        public ActionResult Update(NgoViewModel nvm, FormCollection collection)
        {
            try
            {
                if (nvm.nm.ngoId == 0)
                {


                    var x = Convert.ToInt32(Session["NgoId"].ToString());
                    var tbl = db.NgoMasters.Where(u => u.ngoId == x).FirstOrDefault();
                    tbl.name = nvm.nm.name;
                    tbl.panNo = nvm.nm.panNo;
                    tbl.uploadPan = nvm.nm.uploadPan;
                    tbl.acheivement = nvm.nm.acheivement;
                    tbl.parentOrg = nvm.nm.parentOrg;
                    tbl.websiteUrl = nvm.nm.websiteUrl;
                    tbl.createdOn = Convert.ToDateTime(DateTime.Now);
                    tbl.createdBy = nvm.nm.createdBy;
                    tbl.isActive = true;
                    tbl.isApproved = false;
                    tbl.isDeleted = false;

                   

                    AddressMaster tbl1 = new AddressMaster();

                    tbl1.addressId = nvm.am.addressId;
                    tbl.addressId = nvm.am.addressId;

                    tbl1.addressType = nvm.am.addressType;
                    tbl1.addressLine1 = nvm.am.addressLine1;
                    tbl1.addressLine2 = nvm.am.addressLine2;
                    tbl1.addressLine3 = nvm.am.addressLine3;
                    tbl1.pincode = nvm.am.pincode;
                   
                  
                    tbl1.countryId = nvm.am.countryId;
                    tbl1.stateId = nvm.am.stateId;
                    tbl1.cityId = nvm.am.cityId;
                    db.AddressMasters.Add(tbl1);
                    db.SaveChanges();
                    return RedirectToAction("NgoRegistration", "Ngo");
                   
                    
                }
                else
                {
                    var tbl = db.NgoMasters.Where(u => u.ngoId == nvm.nm.ngoId).FirstOrDefault();
                    tbl.name = nvm.nm.name;
                   
                    tbl.panNo = nvm.nm.panNo;
                    // tbl.uploadPan = nvm.nm.uploadPan;

                    if(nvm.File!=null)
                    {
                        var uploadfile = new byte[nvm.File.InputStream.Length];
                        nvm.File.InputStream.Read(uploadfile, 0, uploadfile.Length);
                        tbl.uploadPan = uploadfile;
                    }
                  



                    tbl.parentOrg = nvm.nm.parentOrg;
                    tbl.acheivement = nvm.nm.acheivement;
                    tbl.websiteUrl = nvm.nm.websiteUrl;
                    tbl.updatedOn = Convert.ToDateTime(DateTime.Now);
                    tbl.updatedBy = Session["EmailId"].ToString();

                    if(tbl.addressId != null)
                    {
                        var tbl1 = db.AddressMasters.Find(tbl.addressId);
                        tbl1.addressType = nvm.am.addressType;
                        tbl1.addressLine1 = nvm.am.addressLine1;
                        tbl1.addressLine2 = nvm.am.addressLine2;
                        tbl1.addressLine3 = nvm.am.addressLine3;
                        tbl1.pincode = nvm.am.pincode;
                        tbl1.cityId = nvm.am.cityId;
                        tbl1.stateId = nvm.am.stateId;
                        tbl1.countryId = nvm.am.countryId;


                    }



                    //if (Request.Files["nvm_nm_uploadPan"] != null)
                    //{
                    //    string Path = "/uploads/" + DateTime.Now.Ticks.ToString() + "_" + Request.Files["nv_nm_uploadPan"].FileName;
                    //    Request.Files["nvm_nm_uploadPan"].SaveAs(Server.MapPath(Path));
                    //    tbl.uploadPan = Path;
                    //}

                    

                    db.SaveChanges();
                    return RedirectToAction("UserNgoDetails", "Ngo");

                }


               

            }
            catch (Exception)
            {

                throw;
            }

          
        }

        [HandleError]
        public ActionResult Delete(int id)
        {
            try
            {
             
                if(id != null)
                {
                    //  working

                    //var x = db.NgoMasters.Find(id);
                    //var y = db.AddressMasters.Find(x.addressId);

                    //db.AddressMasters.Remove(y);
                    //db.NgoMasters.Remove(x);
                    //db.SaveChanges();



                    //IS WORKINIG FOR T/F

                    var tbl = db.NgoMasters.Where(u => u.ngoId == id).FirstOrDefault();
                    tbl.isActive = false;
                    tbl.isDeleted = true;
                    tbl.deletedOn = Convert.ToDateTime(DateTime.Now);
                    tbl.deletedBy = Session["EmailId"].ToString();
                    db.SaveChanges();


                }

                return RedirectToAction("NgoRegistration");
            }
            catch (Exception)
            {
                
                throw;
            }


        }

        [HandleError]
        public ActionResult GetId(int id)
        {
            try
            {
                    NgoViewModel nvm = new NgoViewModel();
                    NgoModel s = new NgoModel();
                    var tbl = db.NgoMasters.Where(u => u.ngoId == id).FirstOrDefault();
                    s.ngoId = tbl.ngoId;
                    s.name = tbl.name;
                    s.emailId = tbl.emailId;
                    s.password = tbl.password;
                    s.panNo = tbl.panNo;
                    s.uploadPan = tbl.uploadPan;
                    s.parentOrg = tbl.parentOrg;
                    s.acheivement = tbl.acheivement;
                    s.websiteUrl = tbl.websiteUrl;
                    s.isActive = tbl.isActive;
                    s.isApproved = tbl.isApproved;
                    s.isDeleted = tbl.isDeleted;
                    s.addressId = tbl.addressId;

                    nvm.nm = s;

                    AddressModel a = new AddressModel();


                    if(tbl.addressId != null)
                        {
                            var tbl1 = db.AddressMasters.Where(u => u.addressId == tbl.addressId).FirstOrDefault();



                            a.addressType = tbl1.addressType;
                            a.addressLine1 = tbl1.addressLine1;
                            a.addressLine2 = tbl1.addressLine2;
                            a.addressLine3 = tbl1.addressLine3;
                            a.pincode = tbl1.pincode;
                            a.countryId = tbl1.countryId;
                            a.stateId = tbl1.stateId;
                            a.cityId = tbl1.cityId;

                            nvm.am = a;

                            nvm.Countries = GetCountry(tbl1.countryId);
                            nvm.States = GetState(tbl1.stateId);
                            nvm.Cities = GetCity(tbl1.cityId);
                      }

                  

                nvm.Countries = PopulateCountryDropDown("CountryName", "CountryId");



                return View("UserNgoRegistration", nvm);

                
            }
            catch (Exception)
            {

                throw;
            }
          
        }


        [HandleError]
        public JsonResult AjaxMethod(string type, int value)
        {
            NgoViewModel model = new NgoViewModel();

            model.Countries = PopulateCountryDropDown("CountryName", "CountryId");
         //   model.States = PopulateStateDropDown(value);
            model.Cities = PopulateCityDropDown(value);
            model.States = PopulateStatedd();

        
           

            return Json(model);
        }

        public List<SelectListItem> PopulateCountryDropDown(string textColumn, string valueColumn)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            
            var list = db.CountryMasters.ToList();
            foreach (var x in list)
            {
                items.Add(new SelectListItem
                {
                    Text = x.countryName,
                    Value = x.countryId.ToString()
                });
            }
            return items;
        }

        public List<SelectListItem> PopulateStateDropDown(int? Sid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            int a = Convert.ToInt32(Sid);
            var lst = db.StateMasters.Where(u => u.countryId == a).ToList();

            foreach (var x in lst)
            {
                items.Add(new SelectListItem
                {
                    Text = x.stateName,
                    Value = x.stateId.ToString()
                });
            }
            return items;
        }



        public List<SelectListItem> PopulateCityDropDown(int? ccid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            int a = Convert.ToInt32(ccid);
            var lst = db.CityMasters.Where(u => u.stateId == a).ToList();

            foreach (var x in lst)
            {
                items.Add(new SelectListItem
                {
                    Text = x.cityName,
                    Value = x.cityId.ToString()
                });
            }
            return items;
        }



        public List<SelectListItem> GetCountry(int? cid)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var list = db.CountryMasters.Where(u=>u.countryId==cid).ToList();
            foreach (var x in list)
            {
                items.Add(new SelectListItem
                {
                    Text = x.countryName,
                    Value = x.countryId.ToString()
                });
            }
            return items;
        }


        public List<SelectListItem> GetState(int? Sid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            
            var lst = db.StateMasters.Where(u => u.stateId==Sid).ToList();

            foreach (var x in lst)
            {
                items.Add(new SelectListItem
                {
                    Text = x.stateName,
                    Value = x.stateId.ToString()
                });
            }
            return items;
        }

        public List<SelectListItem> GetCity(int? ccid)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            
            var lst = db.CityMasters.Where(u => u.cityId == ccid).ToList();

            foreach (var x in lst)
            {
                items.Add(new SelectListItem
                {
                    Text = x.cityName,
                    Value = x.cityId.ToString()
                });
            }
            return items;
        }




        


        public ActionResult NgoList()
        {

            NgoRegistration();
            return View();
        }



        public ActionResult NgoApprovedList()
        {
            try
            {


                NgoViewModel nvm = new NgoViewModel();
                List<NgoModel> obj = new List<NgoModel>();
                var list = db.NgoMasters.Where(u=>u.isApproved==true && u.isDeleted==false).ToList();
                foreach (var x in list)
                {
                    NgoModel S = new NgoModel();
                    S.ngoId = x.ngoId;
                    S.name = x.name;
                    S.emailId = x.emailId;
                    S.password = x.password;
                    S.panNo = x.panNo;
                    S.acheivement = x.acheivement;
                    S.uploadPan = x.uploadPan;
                    S.parentOrg = x.parentOrg;
                    S.websiteUrl = x.websiteUrl;
                    S.createdOn = x.createdOn;
                    S.createdBy = x.createdBy;
                    S.updatedOn = x.updatedOn;
                    S.updatedBy = x.updatedBy;
                    S.deletedOn = x.deletedOn;
                    S.deletedBy = x.createdBy;
                    S.isActive = x.isActive;
                    S.isApproved = x.isApproved;
                    S.isDeleted = x.isDeleted;

                    S.AddressMaster = db.AddressMasters.Find(S.addressId);
                    obj.Add(S);

                }

             

                nvm.listnm = obj;
                
                return View("NgoApprovedList", nvm);

            }
            catch (Exception)
            {

                throw;
            }
        }




        public ActionResult NgoPendingList()
        {
            try
            {


                NgoViewModel nvm = new NgoViewModel();
                List<NgoModel> obj = new List<NgoModel>();
                var list = db.NgoMasters.Where(u => u.isApproved == false && u.isDeleted== false).ToList();
                foreach (var x in list)
                {
                    NgoModel S = new NgoModel();
                    S.ngoId = x.ngoId;
                    S.name = x.name;
                    S.emailId = x.emailId;
                    S.password = x.password;
                    S.panNo = x.panNo;
                    S.acheivement = x.acheivement;
                    S.uploadPan = x.uploadPan;
                    S.parentOrg = x.parentOrg;
                    S.websiteUrl = x.websiteUrl;
                    S.createdOn = x.createdOn;
                    S.createdBy = x.createdBy;
                    S.updatedOn = x.updatedOn;
                    S.updatedBy = x.updatedBy;
                    S.deletedOn = x.deletedOn;
                    S.deletedBy = x.createdBy;
                    S.isActive = x.isActive;
                    S.isApproved = x.isApproved;
                    S.isDeleted = x.isDeleted;

                    S.AddressMaster = db.AddressMasters.Find(S.addressId);
                    obj.Add(S);

                }


                nvm.listnm = obj;

                return View("NgoPendingList", nvm);

            }
            catch (Exception)
            {

                throw;
            }
        }


        public ActionResult NgoAdmin()
        {

            NgoRegistration();
            return View();
        }


        [HandleError]
        public ActionResult GetDetailsId(int id)
        {
            try
            {
                NgoViewModel nvm = new NgoViewModel();
                NgoModel s = new NgoModel();
                var tbl = db.NgoMasters.Where(u => u.ngoId == id).FirstOrDefault();
                s.ngoId = tbl.ngoId;
                s.name = tbl.name;
                s.emailId = tbl.emailId;
                s.password = tbl.password;
                s.panNo = tbl.panNo;
                s.uploadPan = tbl.uploadPan;
                s.parentOrg = tbl.parentOrg;
                s.acheivement = tbl.acheivement;
                s.websiteUrl = tbl.websiteUrl;
                s.createdBy = tbl.createdBy;
                s.createdOn = tbl.createdOn;
                s.updatedBy = tbl.updatedBy;
                s.updatedOn = tbl.updatedOn;
                s.isActive = tbl.isActive;
                s.isApproved = tbl.isApproved;
                s.isDeleted = tbl.isDeleted;
                s.addressId = tbl.addressId;
                s.workingSector = tbl.workingSector;

               
                if(s.fcraId != null)
                {
                    s.FcraDetail = db.FcraDetails.Find(s.fcraId);
                }
               




                List<FcraModel> fvmobj = new List<FcraModel>();
                var fvmlist = db.FcraDetails.Where(u=>u.fcraId == s.fcraId).ToList();
                foreach (var x in fvmlist)
                {
                    FcraModel F = new FcraModel();
                    F.fcraId = x.fcraId;
                    F.fcraRegNo = x.fcraRegNo;
                    F.regdate = x.regdate;
                    F.validUpTo = x.validUpTo;

                    fvmobj.Add(F);

                }


                nvm.listfcra = fvmobj;




                nvm.nm = s;





                if (s.addressId != null)
                {
                    AddressModel a = new AddressModel();


                    var tbl1 = db.AddressMasters.Where(u => u.addressId == tbl.addressId).FirstOrDefault();

                    a.addressType = tbl1.addressType;
                    a.addressLine1 = tbl1.addressLine1;
                    a.addressLine2 = tbl1.addressLine2;
                    a.addressLine3 = tbl1.addressLine3;
                    a.pincode = tbl1.pincode;
                    a.countryId = tbl1.countryId;
                    a.stateId = tbl1.stateId;
                    a.cityId = tbl1.cityId;


                    CountryModel c = new CountryModel();

                    if(tbl1.countryId != null)
                    {
                        var tblcountry = db.CountryMasters.Where(u => u.countryId == tbl1.countryId).FirstOrDefault();
                        c.countryName = tblcountry.countryName;
                    }
                 
                    StateModel st = new StateModel();


                    if (tbl1.stateId != null)
                    {
                        var tblstate = db.StateMasters.Where(u => u.stateId == tbl1.stateId).FirstOrDefault();
                        st.stateName = tblstate.stateName;
                    }
                  

                    CityModel ci = new CityModel();
                    if(tbl1.cityId != null)
                    { 
                        var tblcity = db.CityMasters.Where(u => u.cityId == tbl1.cityId).FirstOrDefault();

                        ci.cityName = tblcity.cityName;
                    }

                   
                    List<ContactModal> cobj = new List<ContactModal>();
                    var conlist = db.ContactMasters.Where(u=>u.ngoId == id).ToList();
                    foreach (var x in conlist)
                    {
                        ContactModal ncm = new ContactModal();
                        ncm.contactId = x.contactId;
                        ncm.contactType = x.contactType;
                        ncm.mobileNo = x.mobileNo;
                        ncm.landlineNo = x.landlineNo;
                        ncm.faxNo = x.faxNo;
                        ncm.ngoId = x.ngoId;


                        cobj.Add(ncm);

                    }


                    nvm.listcotact = cobj;

                    List<RegisModel> regisobj = new List<RegisModel>();
                    var rvmlist = db.RegisDetails.Where(u => u.ngoId == id).ToList();
                    foreach (var x in rvmlist)
                    {
                        RegisModel R = new RegisModel();
                        R.regId = x.regId;
                        R.regWith = x.regWith;
                        R.type = x.type;
                        R.validUpTo = x.validUpTo;
                        R.city = x.city;
                        R.state = x.state;
                        R.regNo = x.regNo;
                        R.regCopy = x.regCopy;
                        R.ngoId = x.ngoId;
                        R.regDate = x.regDate;
                      

                        regisobj.Add(R);

                    }


                    nvm.listRegis = regisobj;


                    List<StaffModel> sobj = new List<StaffModel>();
                    var slist = db.StaffMasters.Where(u=>u.ngoId ==id).ToList();
                    foreach (var x in slist)
                    {
                        StaffModel nst = new StaffModel();
                        nst.staffId = x.staffId;
                        nst.firstName = x.firstName;
                        nst.middleName = x.middleName;
                        nst.lastName = x.lastName;
                        nst.designation = x.designation;
                        nst.emailId = x.emailId;
                        nst.phoneNo = x.phoneNo;
                       

                        sobj.Add(nst);

                    }


                    nvm.liststaff = sobj;


                    nvm.cm = c;
                    nvm.sm = st;
                    nvm.citym = ci;

                    nvm.am = a;

                    nvm.Countries = GetCountry(tbl1.countryId);
                    nvm.States = GetState(tbl1.stateId);
                    nvm.Cities = GetCity(tbl1.cityId);

                }



                if (Session["RoId"].ToString() == "4")
                {
                    return View("NgoDetails", nvm);
                }
                else
                {
                    return View("UserViewNgoDetails", nvm);
                }
                





            }
            catch (Exception)
            {

                throw;
            }

        }

        

      



        public ActionResult  ApprovedNgo(int id)
        {
            
            try
            {
                NgoViewModel nvm = new NgoViewModel();
               
                var tbl = db.NgoMasters.Where(u => u.ngoId == id).FirstOrDefault();
                tbl.isApproved = true;
                db.SaveChanges();

                return RedirectToAction("NgoList","Ngo");

            }

            catch(Exception)
            {
                throw;
            }

              
        }



        //Address:

       

        [HttpPost]
        public ActionResult AddUpdate(AddressViewModel avm)
        {

            if (avm.am.addressId == 0)
            {

                AddressMaster tbl = new AddressMaster();
                tbl.addressType = avm.am.addressType;
                tbl.addressLine1 = avm.am.addressLine1;
                tbl.addressLine2 = avm.am.addressLine2;
                tbl.addressLine3 = avm.am.addressLine3;
                tbl.pincode = avm.am.pincode;
                tbl.cityId = avm.am.cityId;
                tbl.stateId = avm.am.stateId;
                tbl.countryId = avm.am.countryId;


                db.AddressMasters.Add(tbl);
                db.SaveChanges();

            }
            else
            {
                var tbl = db.AddressMasters.Where(u => u.addressId == avm.am.addressId).FirstOrDefault();
                tbl.addressType = avm.am.addressType;
                tbl.addressLine1 = avm.am.addressLine1;
                tbl.addressLine2 = avm.am.addressLine2;
                tbl.addressLine3 = avm.am.addressLine3;
                tbl.pincode = avm.am.pincode;
                tbl.cityId = avm.am.cityId;
                tbl.stateId = avm.am.stateId;
                tbl.countryId = avm.am.countryId;
                db.SaveChanges();



            }

            // return View("Index", nvm);
            return RedirectToAction("NgoRegistration");

        }


        public ActionResult AddDelete(int id)
        {

            var x = db.AddressMasters.Find(id);
            db.AddressMasters.Remove(x);
            db.SaveChanges();

            //IS WORKINIG FOR T/F

            //var tbl = db.NgoMasters.Where(u => u.ngoId == id).FirstOrDefault();
            //tbl.isActive = false;
            //tbl.isDeleted = false;
            //db.SaveChanges();

            return RedirectToAction("NgoRegistration");
        }


        public ActionResult AdddGetId(int id)
        {
            AddressViewModel avm = new AddressViewModel();
            AddressModel s = new AddressModel();
            var tbl = db.AddressMasters.Where(u => u.addressId == id).FirstOrDefault();
            s.addressId = tbl.addressId;
            s.addressType = tbl.addressType;
            s.addressLine1 = tbl.addressLine1;
            s.addressLine2 = tbl.addressLine2;
            s.addressLine3 = tbl.addressLine3;
            s.pincode = tbl.pincode;
            s.cityId = tbl.cityId;
            s.stateId = tbl.stateId;
            s.countryId = tbl.countryId;

            avm.am = s;

            List<AddressModel> obj = new List<AddressModel>();
            var list = db.AddressMasters.ToList();
            foreach (var x in list)
            {
                AddressModel S = new AddressModel();
                S.addressId = x.addressId;
                S.addressType = x.addressType;
                S.addressLine1 = x.addressLine1;
                S.addressLine2 = x.addressLine2;
                S.addressLine3 = x.addressLine3;
                S.pincode = x.pincode;
                S.cityId = x.cityId;
                S.stateId = x.stateId;
                S.countryId = x.countryId;
                obj.Add(S);

            }


            avm.listam = obj;

            return View("NgoRegistration", avm);

        }






        [HttpPost]
        public ActionResult FcraUpdate(FcraViewModel fvm)
        {

            if (fvm.fm.fcraId == 0)
            {

                FcraDetail tbl = new FcraDetail();
              
                tbl.fcraRegNo = fvm.fm.fcraRegNo;
                tbl.regdate = fvm.fm.regdate;
                tbl.validUpTo = fvm.fm.validUpTo;

                var n = Convert.ToInt32(Session["NgoId"].ToString());
                var tbl1 = db.NgoMasters.Where(u => u.ngoId == n).FirstOrDefault();
                tbl1.fcraId = tbl.fcraId;
               

                db.FcraDetails.Add(tbl);
                db.SaveChanges();

            }
            else
            {
                var tbl = db.FcraDetails.Where(u => u.fcraId == fvm.fm.fcraId).FirstOrDefault();
                tbl.fcraId = fvm.fm.fcraId;
                tbl.fcraRegNo = fvm.fm.fcraRegNo;
                tbl.regdate = fvm.fm.regdate;
                tbl.validUpTo = fvm.fm.validUpTo;


                db.SaveChanges();



            }

            // return View("Index", nvm);
            return RedirectToAction("UserFcraList");

        }


        public ActionResult FcraDelete(int id)
        {

            var x = db.FcraDetails.Find(id);
            db.FcraDetails.Remove(x);
            db.SaveChanges();
            return RedirectToAction("NgoRegistration");
        }


        public ActionResult FcraGetId(int id)
        {
            FcraViewModel fvm = new FcraViewModel();
            FcraModel s = new FcraModel();
            var tbl = db.FcraDetails.Where(u => u.fcraId == id).FirstOrDefault();
            s.fcraId = tbl.fcraId;
            s.fcraRegNo = tbl.fcraRegNo;
            s.regdate = tbl.regdate;
            s.validUpTo = tbl.validUpTo;
            fvm.fm = s;

            List<FcraModel> fvmobj = new List<FcraModel>();
            var fvmlist = db.FcraDetails.ToList();
            foreach (var x in fvmlist)
            {
                FcraModel F = new FcraModel();
                F.fcraId = x.fcraId;
                F.fcraRegNo = x.fcraRegNo;
                F.regdate = x.regdate;
                F.validUpTo = x.validUpTo;

                fvmobj.Add(F);

            }


            fvm.listfm = fvmobj;
            // return View(fvm);

            return View("UserFcra", fvm);

        }




      
        [HttpPost]
        public ActionResult RegisUpdate(RegisViewModel rvm)
        {

            if (rvm.rm.regId == 0)
            {

                RegisDetail tbl = new RegisDetail();
                tbl.regWith = rvm.rm.regWith;
                tbl.type = rvm.rm.type;
                tbl.city = rvm.rm.city;
                tbl.state = rvm.rm.state;
                tbl.regDate = rvm.rm.regDate;
                tbl.validUpTo = rvm.rm.validUpTo;
                tbl.regNo = rvm.rm.regNo;
                // tbl.regCopy = rvm.rm.regCopy;

                if(rvm.File != null)
                {
                    var uploadfile = new byte[rvm.File.InputStream.Length];
                    rvm.File.InputStream.Read(uploadfile, 0, uploadfile.Length);
                    tbl.regCopy = uploadfile;
                }
            


                tbl.ngoId = Convert.ToInt32(Session["NgoId"].ToString()); 
                db.RegisDetails.Add(tbl);
                db.SaveChanges();

            }
            else
            {
                var tbl = db.RegisDetails.Where(u => u.regId == rvm.rm.regId).FirstOrDefault();
                tbl.regWith = rvm.rm.regWith;
                tbl.type = rvm.rm.type;
                tbl.city = rvm.rm.city;
                tbl.state = rvm.rm.state;
                tbl.regDate = rvm.rm.regDate;
                tbl.validUpTo = rvm.rm.validUpTo;
                tbl.regNo = rvm.rm.regNo;
                // tbl.regCopy = rvm.rm.regCopy;


                if (rvm.File != null)
                {
                    var uploadfile = new byte[rvm.File.InputStream.Length];
                    rvm.File.InputStream.Read(uploadfile, 0, uploadfile.Length);
                    tbl.regCopy = uploadfile;

                }
                db.SaveChanges();


            }

            // return View("Index", nvm);
            return RedirectToAction("UserRegisList");

        }


        public ActionResult RegisDelete(int id)
        {

            var x = db.RegisDetails.Find(id);
            db.RegisDetails.Remove(x);
            db.SaveChanges();
            return RedirectToAction("UserRegisList");
        }


        public ActionResult RegisGetId(int id)
        {
            RegisViewModel rvm = new RegisViewModel();
            RegisModel r = new RegisModel();
            var tbl = db.RegisDetails.Where(u => u.regId == id).FirstOrDefault();
            r.regId = tbl.regId;
            r.regWith = tbl.regWith;
            r.type = tbl.type;
            r.city = tbl.city;
            r.state = tbl.state;
            r.regDate = tbl.regDate;
            r.validUpTo = tbl.validUpTo;
            r.regNo = tbl.regNo;
            r.regCopy = tbl.regCopy;
            r.ngoId = tbl.ngoId;

            rvm.rm = r;


            List<RegisModel> rvmobj = new List<RegisModel>();
            var rvmlist = db.RegisDetails.ToList();
            foreach (var x in rvmlist)
            {
                RegisModel R = new RegisModel();
                R.regId = x.regId;
                R.regWith = x.regWith;
                R.type = x.type;
                R.validUpTo = x.validUpTo;
                R.city = x.city;
                R.state = x.state;
                R.regDate = x.regDate;
                R.validUpTo = x.validUpTo;
                R.regNo = x.regNo;
                R.regCopy = x.regCopy;
                r.ngoId = x.ngoId;
                rvmobj.Add(R);

            }


            rvm.listrm = rvmobj;

            return View("UserRegis", rvm);

        }



        public ActionResult UserSector()
        {

            try
            {

                NgoViewModel nvm = new NgoViewModel();
                List<WorkingModel> obj = new List<WorkingModel>();
                var list = db.WorkingSectors.ToList();
                foreach (var x in list)
                {
                    WorkingModel S = new WorkingModel();
                    S.wsId = x.wsId;
                    S.Name = x.Name;
                   
                    obj.Add(S);

                }
                nvm.listwm = obj;


                
                List<NgoModel> nobj = new List<NgoModel>();
                var n = Convert.ToInt32(Session["NgoId"].ToString());
                var nlist = db.NgoMasters.Where(u => u.ngoId == n).ToList();

                foreach (var x in nlist)
                {
                    NgoModel T = new NgoModel();
                    T.ngoId = x.ngoId;
                    T.workingSector = x.workingSector;

                    nobj.Add(T);

                }
                nvm.listnm = nobj;

                return View(nvm);

            }
            catch (Exception)
            {

                throw;
            }


           
        }





        [HandleError]
        public ActionResult SectorGetId(int id)
        {
            try
            {
                NgoViewModel nvm = new NgoViewModel();
                NgoModel s = new NgoModel();
                
                var n = Convert.ToInt32(Session["NgoId"].ToString());
                var nlist = db.NgoMasters.Where(u => u.ngoId == n).FirstOrDefault();

                s.workingSector = nlist.workingSector;
                s.ngoId = nlist.ngoId;


                nvm.nm = s;


               
                List<WorkingModel> obj = new List<WorkingModel>();
                var list = db.WorkingSectors.ToList();
                foreach (var x in list)
                {
                    WorkingModel S = new WorkingModel();
                    S.wsId = x.wsId;
                    S.Name = x.Name;

                    obj.Add(S);

                }
                nvm.listwm = obj;

                return View("UserSector", nvm);


            }
            catch (Exception)
            {

                throw;
            }

        }



        public ActionResult SectorUpdate(NgoViewModel nvm)
        {

            try
            {
                if(Session["NgoId"] != null)
                {
                    var x = Convert.ToInt32(Session["NgoId"].ToString());
                    var tbl = db.NgoMasters.Where(u => u.ngoId == x).FirstOrDefault();
                   
                    tbl.workingSector = nvm.nm.workingSector;


                    db.SaveChanges();
                    return RedirectToAction("UserSector", "Ngo");
                }
                else
                {
                    return RedirectToAction("Login", "User");
                }
               
              
            }
            catch (Exception)
            {

                throw;
            }

        }




        //WorkingSector


        public ActionResult WorkingSector()
        {

            WorkingViewModel wvm = new WorkingViewModel();
            List<WorkingModel> wvmobj = new List<WorkingModel>();
            var wvmlist = db.WorkingSectors.ToList();
            foreach (var x in wvmlist)
            {
                WorkingModel w = new WorkingModel();
                w.wsId = x.wsId;
                w.Name = x.Name;
                w.Description = x.Description;
                w.ceratedBy = x.ceratedBy;
                w.createdOn = x.createdOn;

                wvmobj.Add(w);

            }


            wvm.listwm = wvmobj;
            return View(wvm);

        }

        [HttpPost]
        public ActionResult WorkingUpdate(WorkingViewModel wvm)
        {

            if (wvm.wm.wsId == 0)
            {

                WorkingSector tbl = new WorkingSector();

                tbl.Name = wvm.wm.Name;
                tbl.Description = wvm.wm.Description;
                tbl.ceratedBy = wvm.wm.ceratedBy;
                tbl.createdOn = wvm.wm.createdOn;


                db.WorkingSectors.Add(tbl);
                db.SaveChanges();

            }
            else
            {
                var tbl = db.WorkingSectors.Where(u => u.wsId == wvm.wm.wsId).FirstOrDefault();
                tbl.Name = wvm.wm.Name;
                tbl.Description = wvm.wm.Description;
                tbl.ceratedBy = wvm.wm.ceratedBy;
                tbl.createdOn = wvm.wm.createdOn;

                db.SaveChanges();


            }

            // return View("Index", nvm);
            return RedirectToAction("NgoRegistration");

        }


        public ActionResult WorkingDelete(int id)
        {

            var x = db.WorkingSectors.Find(id);
            db.WorkingSectors.Remove(x);
            db.SaveChanges();
            return RedirectToAction("NgoRegistration");
        }


        public ActionResult WorkingGetId(int id)
        {
            WorkingViewModel wvm = new WorkingViewModel();
            WorkingModel w = new WorkingModel();
            var tbl = db.WorkingSectors.Where(u => u.wsId == id).FirstOrDefault();
            w.wsId = tbl.wsId;
            w.Name = tbl.Name;
            w.Description = tbl.Description;
            w.ceratedBy = tbl.ceratedBy;
            w.createdOn = tbl.createdOn;



            wvm.wm = w;


            List<WorkingModel> wvmobj = new List<WorkingModel>();
            var wvmlist = db.WorkingSectors.ToList();
            foreach (var x in wvmlist)
            {
                WorkingModel work = new WorkingModel();
                work.wsId = x.wsId;
                work.Name = x.Name;
                work.Description = x.Description;
                work.ceratedBy = x.ceratedBy;
                work.createdOn = x.createdOn;

                wvmobj.Add(work);

            }


            wvm.listwm = wvmobj;

            return View("NgoRegistration", wvm);

        }




        //Country



        public ActionResult Country()
        {

            CountryViewModel cvm = new CountryViewModel();
            List<CountryModel> cobj = new List<CountryModel>();
            var clist = db.CountryMasters.ToList();
            foreach (var x in clist)
            {
                CountryModel c = new CountryModel();
                c.countryId = c.countryId;
                c.countryName = x.countryName;


                cobj.Add(c);

            }


            cvm.listcm = cobj;
            return View(cvm);

        }

        [HttpPost]
        public ActionResult CoutryUpdate(CountryViewModel cvm)
        {

            if (cvm.cm.countryId == 0)
            {

                CountryMaster tbl = new CountryMaster();

                tbl.countryName = cvm.cm.countryName;

                db.CountryMasters.Add(tbl);
                db.SaveChanges();

            }
            else
            {
                var tbl = db.CountryMasters.Where(u => u.countryId == cvm.cm.countryId).FirstOrDefault();
                tbl.countryName = cvm.cm.countryName;

                db.SaveChanges();


            }

            // return View("Index", nvm);
            return RedirectToAction("NgoRegistration");

        }


        public ActionResult CountryDelete(int id)
        {

            var x = db.CountryMasters.Find(id);
            db.CountryMasters.Remove(x);
            db.SaveChanges();
            return RedirectToAction("NgoRegistration");
        }


        public ActionResult CountryGetId(int id)
        {
            CountryViewModel cvm = new CountryViewModel();
            CountryModel w = new CountryModel();
            var tbl = db.CountryMasters.Where(u => u.countryId == id).FirstOrDefault();
            w.countryId = tbl.countryId;
            w.countryName = tbl.countryName;


            cvm.cm = w;


            List<CountryModel> cobj = new List<CountryModel>();
            var clist = db.CountryMasters.ToList();
            foreach (var x in clist)
            {
                CountryModel c = new CountryModel();
                c.countryId = c.countryId;
                c.countryName = x.countryName;


                cobj.Add(c);

            }


            cvm.listcm = cobj;



            return View("NgoRegistration", cvm);

        }





        //State



        public ActionResult State()
        {

            StateViewModel svm = new StateViewModel();
            List<StateModel> sobj = new List<StateModel>();
            var slist = db.StateMasters.ToList();
            foreach (var x in slist)
            {
                StateModel s = new StateModel();
                s.stateId = x.stateId;
                s.stateName = x.stateName;

                sobj.Add(s);

            }


            svm.listsm = sobj;
            return View(svm);

        }

        [HttpPost]
        public ActionResult StateUpdate(StateViewModel svm)
        {

            if (svm.sm.stateId == 0)
            {

                StateMaster tbl = new StateMaster();

                tbl.stateName = svm.sm.stateName;

                db.StateMasters.Add(tbl);
                db.SaveChanges();

            }
            else
            {
                var tbl = db.StateMasters.Where(u => u.stateId == svm.sm.stateId).FirstOrDefault();
                tbl.stateName = svm.sm.stateName;

                db.SaveChanges();


            }

            // return View("Index", svm);
            return RedirectToAction("NgoRegistration");

        }


        public ActionResult StateDelete(int id)
        {

            var x = db.StateMasters.Find(id);
            db.StateMasters.Remove(x);
            db.SaveChanges();
            return RedirectToAction("NgoRegistration");
        }


        public ActionResult StateGetId(int id)
        {
            StateViewModel svm = new StateViewModel();
            StateModel s = new StateModel();
            var tbl = db.StateMasters.Where(u => u.stateId == id).FirstOrDefault();
            s.stateId = tbl.stateId;
            s.stateName = tbl.stateName;


            svm.sm = s;


            List<StateModel> sobj = new List<StateModel>();
            var slist = db.StateMasters.ToList();
            foreach (var x in slist)
            {
                StateModel smo = new StateModel();
                smo.stateId = x.stateId;
                smo.stateName = x.stateName;

                sobj.Add(smo);

            }


            svm.listsm = sobj;



            return View("NgoRegistration", svm);

        }




        //city

        public ActionResult City()
        {

            CityViewModel cityvm = new CityViewModel();
            List<CityModel> cityobj = new List<CityModel>();
            var citylist = db.CityMasters.ToList();
            foreach (var x in citylist)
            {
                CityModel c = new CityModel();
                c.cityId = x.cityId;
                c.cityName = x.cityName;

                cityobj.Add(c);

            }


            cityvm.listcitym = cityobj;
            return View(cityvm);

        }

        [HttpPost]
        public ActionResult CityUpdate(CityViewModel cvm)
        {

            if (cvm.citym.cityId == 0)
            {

                CityMaster tbl = new CityMaster();

                tbl.cityName = cvm.citym.cityName;

                db.CityMasters.Add(tbl);
                db.SaveChanges();

            }
            else
            {
                var tbl = db.CityMasters.Where(u => u.cityId == cvm.citym.cityId).FirstOrDefault();
                tbl.cityName = cvm.citym.cityName;

                db.SaveChanges();


            }

            // return View("Index", svm);
            return RedirectToAction("NgoRegistration");

        }


        public ActionResult CityDelete(int id)
        {

            var x = db.CityMasters.Find(id);
            db.CityMasters.Remove(x);
            db.SaveChanges();
            return RedirectToAction("NgoRegistration");
        }


        public ActionResult CityGetId(int id)
        {
            CityViewModel cityvm = new CityViewModel();
            CityModel cm = new CityModel();
            var tbl = db.CityMasters.Where(u => u.cityId == id).FirstOrDefault();
            cm.cityId = tbl.cityId;
            cm.cityName = tbl.cityName;


            cityvm.citym = cm;


            List<CityModel> cityobj = new List<CityModel>();
            var citylist = db.CityMasters.ToList();
            foreach (var x in citylist)
            {
                CityModel c = new CityModel();
                c.cityId = x.cityId;
                c.cityName = x.cityName;

                cityobj.Add(c);

            }


            cityvm.listcitym = cityobj;
            return View("NgoRegistration", cityvm);

        }




     
        [HttpPost]
        public ActionResult StaffUpdate(StaffViewModel svm)
        {

            if (svm.sm.staffId == 0)
            {

                StaffMaster tbl = new StaffMaster();

                tbl.firstName = svm.sm.firstName;
                tbl.middleName = svm.sm.middleName;
                tbl.lastName = svm.sm.lastName;
                tbl.designation = svm.sm.designation;
                tbl.emailId = svm.sm.emailId;
                tbl.phoneNo = svm.sm.phoneNo;
                tbl.ngoId = Convert.ToInt32(Session["NgoId"].ToString()); 


                db.StaffMasters.Add(tbl);
                db.SaveChanges();

            }
            else
            {
                var tbl = db.StaffMasters.Where(u => u.staffId == svm.sm.staffId).FirstOrDefault();
                tbl.firstName = svm.sm.firstName;
                tbl.middleName = svm.sm.middleName;
                tbl.lastName = svm.sm.lastName;
                tbl.designation = svm.sm.designation;
                tbl.emailId = svm.sm.emailId;
                tbl.phoneNo = svm.sm.phoneNo;
               


                db.SaveChanges();


            }

            // return View("Index", svm);
            return RedirectToAction("UserStaffList");

        }


        public ActionResult StaffDelete(int id)
        {

            var x = db.StaffMasters.Find(id);
            db.StaffMasters.Remove(x);
            db.SaveChanges();
            return RedirectToAction("UserStaffList");
        }


        public ActionResult StaffGetId(int id)
        {
            StaffViewModel svm = new StaffViewModel();
            StaffModel sml = new StaffModel();
            var tbl = db.StaffMasters.Where(u => u.staffId == id).FirstOrDefault();
            sml.staffId = tbl.staffId;
            sml.firstName = tbl.firstName;
            sml.middleName = tbl.middleName;
            sml.lastName = tbl.lastName;
            sml.designation = tbl.designation;
            sml.emailId = tbl.emailId;
            sml.phoneNo = tbl.phoneNo;
            sml.ngoId = tbl.ngoId;



            svm.sm = sml;


            List<StaffModel> sobj = new List<StaffModel>();
            var slist = db.StaffMasters.ToList();
            foreach (var x in slist)
            {
                StaffModel s = new StaffModel();
                s.staffId = x.staffId;
                s.firstName = x.firstName;
                s.middleName = x.middleName;
                s.lastName = x.lastName;
                s.designation = x.designation;
                s.emailId = x.emailId;
                s.phoneNo = x.phoneNo;
                s.ngoId = x.ngoId;

                sobj.Add(s);

            }


            svm.listsm = sobj;
            return View("UserStaff", svm);

        }


        [HttpPost]
        public ActionResult ContactUpdate(ContactViewModel cvm)
        {

            if (cvm.con.contactId == 0)
            {

                ContactMaster tbl = new ContactMaster();

                tbl.contactType = cvm.con.contactType;
                tbl.mobileNo = cvm.con.mobileNo;
                tbl.landlineNo = cvm.con.landlineNo;
                tbl.faxNo = cvm.con.faxNo;
                tbl.ngoId = Convert.ToInt32(Session["NgoId"].ToString());


                db.ContactMasters.Add(tbl);
                db.SaveChanges();

            }
            else
            {
                var tbl = db.ContactMasters.Where(u => u.contactId == cvm.con.contactId).FirstOrDefault();

                tbl.contactType = cvm.con.contactType;
                tbl.mobileNo = cvm.con.mobileNo;
                tbl.landlineNo = cvm.con.landlineNo;
                tbl.faxNo = cvm.con.faxNo;
               
                db.SaveChanges();


            }

            // return View("Index", svm);
            return RedirectToAction("UserContactList");

        }


        public ActionResult ContactDelete(int id)
        {

            var x = db.ContactMasters.Find(id);
            db.ContactMasters.Remove(x);
            db.SaveChanges();
            return RedirectToAction("UserContactList");
        }


        public ActionResult ContactGetId(int id)
        {


            ContactViewModel cvm = new ContactViewModel();
            ContactModal cm = new ContactModal();
            var tbl = db.ContactMasters.Where(u => u.contactId == id).FirstOrDefault();

            cm.contactId = tbl.contactId;
            cm.contactType = tbl.contactType;
            cm.mobileNo = tbl.mobileNo;
            cm.landlineNo = tbl.landlineNo;
            cm.faxNo = tbl.faxNo;
            cm.ngoId = tbl.ngoId;
            

            cvm.con = cm;


            List<ContactModal> cobj = new List<ContactModal>();
            var conlist = db.ContactMasters.ToList();
            foreach (var x in conlist)
            {
                ContactModal c = new ContactModal();
                c.contactId = x.contactId;
                c.contactType = x.contactType;
                c.mobileNo = x.mobileNo;
                c.mobileNo = x.mobileNo;
                c.landlineNo = x.landlineNo;
                c.faxNo = x.faxNo;
                c.ngoId = x.ngoId;


                cobj.Add(c);

            }


            cvm.conlist = cobj;
            return View("UserContact", cvm);

        }




        public ActionResult UserNgoDetails()
        {

            try
            {

                if (Session["NgoId"] != null)
                {

                    NgoViewModel nvm = new NgoViewModel();
                    List<NgoModel> obj = new List<NgoModel>();

                    var n = Convert.ToInt32(Session["NgoId"].ToString());
                    var list = db.NgoMasters.Where(u => u.ngoId == n).ToList();



                    foreach (var x in list)
                    {
                        NgoModel S = new NgoModel();
                        S.ngoId = x.ngoId;
                        S.name = x.name;
                        S.emailId = x.emailId;
                        S.password = x.password;

                        S.panNo = x.panNo;
                        S.acheivement = x.acheivement;
                        S.uploadPan = x.uploadPan;
                        S.parentOrg = x.parentOrg;
                        S.websiteUrl = x.websiteUrl;
                        S.createdOn = x.createdOn;
                        S.createdBy = x.createdBy;
                        S.updatedOn = x.updatedOn;
                        S.updatedBy = x.updatedBy;
                        S.deletedOn = x.deletedOn;
                        S.deletedBy = x.createdBy;
                        S.isActive = x.isActive;
                        S.isApproved = x.isApproved;
                        S.isDeleted = x.isDeleted;
                        S.addressId = x.addressId;
                        S.userId = x.userId;

                        S.AddressMaster = db.AddressMasters.Find(S.addressId);

                        S.UserMaster = db.UserMasters.Find(S.userId);

                        obj.Add(S);

                    }



                    List<AddressModel> adobj = new List<AddressModel>();
                    var adlist = db.AddressMasters.ToList();
                    foreach (var x in adlist)
                    {
                        AddressModel A = new AddressModel();
                        A.addressId = x.addressId;
                        A.addressType = x.addressType;
                        A.addressLine1 = x.addressLine1;
                        A.addressLine2 = x.addressLine2;
                        A.addressLine3 = x.addressLine3;
                        A.pincode = x.pincode;
                        A.cityId = x.cityId;
                        A.stateId = x.stateId;
                        A.countryId = x.countryId;

                        //var a = db.CountryMasters.Find();
                        adobj.Add(A);

                    }


                    nvm.listam = adobj;

                    nvm.listnm = obj;
                    nvm.Countries = PopulateCountryDropDown("CountryName", "CountryId");
                    return View(nvm);

                }
                else
                {
                    return RedirectToAction("Login", "User");
                }

            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult UserStaffList()
        {
            if (Session["NgoId"] != null)
            {

                StaffViewModel svm = new StaffViewModel();
                List<StaffModel> sobj = new List<StaffModel>();
                var n = Convert.ToInt32(Session["NgoId"].ToString());
                var slist = db.StaffMasters.Where(u => u.ngoId == n).ToList();

                foreach (var x in slist)
                {
                    StaffModel s = new StaffModel();
                    s.staffId = x.staffId;
                    s.firstName = x.firstName;
                    s.middleName = x.middleName;
                    s.lastName = x.lastName;
                    s.designation = x.designation;
                    s.emailId = x.emailId;
                    s.phoneNo = x.phoneNo;
                    s.ngoId = x.ngoId;

                    sobj.Add(s);

                }


                svm.listsm = sobj;
                return View(svm);

            }
            else
            {
                return RedirectToAction("Login", "User");

            }



        }

      
        public ActionResult UserContactList()
        {
            if (Session["NgoId"] != null)
            {

                ContactViewModel cvm = new ContactViewModel();
                List<ContactModal> cobj = new List<ContactModal>();

                var list = Convert.ToInt32(Session["NgoId"].ToString());
                var clist = db.ContactMasters.Where(u => u.ngoId == list).ToList();


                foreach (var x in clist)
                {
                    ContactModal c = new ContactModal();
                    c.contactId = x.contactId;
                    c.contactType = x.contactType;
                    c.mobileNo = x.mobileNo;
                    c.landlineNo = x.landlineNo;
                    c.faxNo = x.faxNo;
                    c.ngoId = x.ngoId;

                    cobj.Add(c);

                }


                cvm.conlist = cobj;
                return View(cvm);

            }
            else
            {
                return RedirectToAction("Login", "User");

            }



        }
        public ActionResult UserRegisList()
        {
            if (Session["NgoId"] != null)
            {

                RegisViewModel rvm = new RegisViewModel();
                List<RegisModel> rvmobj = new List<RegisModel>();
                var list = Convert.ToInt32(Session["NgoId"].ToString());
                var rvmlist = db.RegisDetails.Where(u => u.ngoId == list).ToList();

                foreach (var x in rvmlist)
                {
                    RegisModel R = new RegisModel();
                    R.regId = x.regId;
                    R.regWith = x.regWith;
                    R.type = x.type;
                    R.validUpTo = x.validUpTo;
                    R.city = x.city;
                    R.state = x.state;
                    R.regNo = x.regNo;
                    R.regCopy = x.regCopy;
                    R.ngoId = x.ngoId;
                    R.regDate = x.regDate;
                    R.ngoId = x.ngoId;

                    rvmobj.Add(R);

                }


                rvm.listrm = rvmobj;
                return View(rvm);

            }
            else
            {
                return RedirectToAction("Login", "User");

            }

        }
        public ActionResult UserFcraList()
        {
            if( Session["NgoId"] != null)
            {
                FcraViewModel fvm = new FcraViewModel();
                List<FcraModel> fvmobj = new List<FcraModel>();

                var list = Convert.ToInt32(Session["NgoId"].ToString());

                var fc = db.NgoMasters.Where(u => u.ngoId == list).FirstOrDefault();

                if (fc.fcraId != null)
                {

                    var x = db.FcraDetails.Where(u => u.fcraId == fc.fcraId).FirstOrDefault();

                    FcraModel F = new FcraModel();
                    F.fcraId = x.fcraId;
                    F.fcraRegNo = x.fcraRegNo;
                    F.regdate = x.regdate;
                    F.validUpTo = x.validUpTo;

                    fvmobj.Add(F);


                }


                fvm.listfm = fvmobj;
                return View(fvm);
            }
            else
            {
                return RedirectToAction("Login","User");

            }
            

           
        }

        public ActionResult UserStaff()
        {
            return View();
        }

        public ActionResult UserRegis()
        {
            RegisViewModel rvm = new RegisViewModel();
            List<RegisModel> rvmobj = new List<RegisModel>();
          
            var rvmlist = db.RegisDetails.ToList();

            foreach (var x in rvmlist)
            {
                RegisModel R = new RegisModel();
                R.regId = x.regId;
                R.regWith = x.regWith;
                R.type = x.type;
                R.validUpTo = x.validUpTo;
                R.city = x.city;
                R.state = x.state;
                R.regNo = x.regNo;
                R.regCopy = x.regCopy;
                R.ngoId = x.ngoId;
                R.regDate = x.regDate;
                R.ngoId = x.ngoId;

                rvmobj.Add(R);

            }


            rvm.listrm = rvmobj;
            return View(rvm);

            
        }
        public ActionResult UserContact()
        {
            return View();
        }
        public ActionResult UserFcra()
        {
            return View();
        }


        //for admin ngo insert
        [HttpPost]
        [HandleError]
        public ActionResult UserEdit(NgoViewModel nvm)
        {
            try
            {
                    NgoMaster tbl = new NgoMaster();
                    tbl.name = nvm.nm.name;
                    tbl.panNo = nvm.nm.panNo;
                    tbl.emailId = nvm.nm.emailId;
                     tbl.password = nvm.nm.password;
                // tbl.uploadPan = nvm.nm.uploadPan;

                if (nvm.File != null)
                {

                    var uploadfile = new byte[nvm.File.InputStream.Length];
                    nvm.File.InputStream.Read(uploadfile, 0, uploadfile.Length);
                    tbl.uploadPan = uploadfile;
                }

                    tbl.acheivement = nvm.nm.acheivement;
                    tbl.parentOrg = nvm.nm.parentOrg;
                    tbl.websiteUrl = nvm.nm.websiteUrl;
                    tbl.createdOn = Convert.ToDateTime(DateTime.Now);
                    tbl.createdBy = nvm.nm.createdBy;
                    tbl.isActive = true;
                    tbl.isApproved = true;
                    tbl.isDeleted = false;

                    db.NgoMasters.Add(tbl);

                    AddressMaster tbl1 = new AddressMaster();

                    tbl1.addressId = nvm.am.addressId;
                    tbl.addressId = nvm.am.addressId;

                    tbl1.addressType = nvm.am.addressType;
                    tbl1.addressLine1 = nvm.am.addressLine1;
                    tbl1.addressLine2 = nvm.am.addressLine2;
                    tbl1.addressLine3 = nvm.am.addressLine3;
                    tbl1.pincode = nvm.am.pincode;


                    tbl1.countryId = nvm.am.countryId;
                    tbl1.stateId = nvm.am.stateId;
                    tbl1.cityId = nvm.am.cityId;
                    db.AddressMasters.Add(tbl1);


                    UserMaster um = new UserMaster();
                    um.userName = tbl.name;
                    um.emailId = tbl.emailId;
                    um.password = nvm.nm.password;
                    um.roleId = 1;
                    um.isActive = true;
                    um.isRegistered = false;
                    tbl.userId = um.userId;

                    db.UserMasters.Add(um);
                
                    db.SaveChanges();

                return RedirectToAction("NgoList", "Ngo");

            }
            catch (Exception)
            {

                throw;
            }


        }


        public ActionResult UserNgoList()
        {
            NgoApprovedList();
            return View();

        }


        public ActionResult UserViewNgoDetails()
        {

            return View();
        }

        public ActionResult ReportNgo()
        {
            NgoViewModel nvm = new NgoViewModel();
            nvm.States = PopulateStatedd();
            return View(nvm);
          
        }

        public ActionResult FilterNgo(NgoViewModel nvm)
        {
            try
            {
               
                List<NgoModel> obj = new List<NgoModel>();


                if(nvm.am.cityId != null && nvm.am.cityId != 0 )
                {
                    var list = db.NgoMasters.Where(u => u.AddressMaster.stateId == nvm.am.stateId && u.AddressMaster.cityId == nvm.am.cityId && u.isApproved == true && u.isDeleted == false).ToList();

                    foreach (var x in list)
                    {
                        NgoModel S = new NgoModel();
                        S.ngoId = x.ngoId;
                        S.name = x.name;
                        S.emailId = x.emailId;
                        S.password = x.password;
                        S.panNo = x.panNo;
                        S.acheivement = x.acheivement;
                        S.uploadPan = x.uploadPan;
                        S.parentOrg = x.parentOrg;
                        S.websiteUrl = x.websiteUrl;
                        S.createdOn = x.createdOn;
                        S.createdBy = x.createdBy;
                        S.updatedOn = x.updatedOn;
                        S.updatedBy = x.updatedBy;
                        S.deletedOn = x.deletedOn;
                        S.deletedBy = x.createdBy;
                        S.isActive = x.isActive;
                        S.isApproved = x.isApproved;
                        S.isDeleted = x.isDeleted;

                        //  S.AddressMaster = db.AddressMasters.Find(S.addressId);
                        obj.Add(S);

                    }



                }

                else
                {
                    var list = db.NgoMasters.Where(u => u.AddressMaster.stateId == nvm.am.stateId && u.isApproved == true && u.isDeleted == false).ToList();

                    foreach (var x in list)
                    {
                        NgoModel S = new NgoModel();
                        S.ngoId = x.ngoId;
                        S.name = x.name;
                        S.emailId = x.emailId;
                        S.password = x.password;
                        S.panNo = x.panNo;
                        S.acheivement = x.acheivement;
                        S.uploadPan = x.uploadPan;
                        S.parentOrg = x.parentOrg;
                        S.websiteUrl = x.websiteUrl;
                        S.createdOn = x.createdOn;
                        S.createdBy = x.createdBy;
                        S.updatedOn = x.updatedOn;
                        S.updatedBy = x.updatedBy;
                        S.deletedOn = x.deletedOn;
                        S.deletedBy = x.createdBy;
                        S.isActive = x.isActive;
                        S.isApproved = x.isApproved;
                        S.isDeleted = x.isDeleted;

                        //  S.AddressMaster = db.AddressMasters.Find(S.addressId);
                        obj.Add(S);

                    }

                }





                nvm.listnm = obj;
                nvm.States = PopulateStatedd();
                return View("ReportNgo", nvm);
                
            }
            catch (Exception)
            {

                throw;
            }


        }



        public List<SelectListItem> PopulateStatedd()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var list = db.StateMasters.ToList();
            foreach (var x in list)
            {
                items.Add(new SelectListItem
                {
                    Text = x.stateName,
                    Value = x.stateId.ToString()
                });
            }
            return items;
        }



        public ActionResult SearchNgo(NgoViewModel nvm)
        {

            List<NgoModel> obj = new List<NgoModel>();

            var list = db.NgoMasters.Where(u => u.name.Contains(nvm.nm.name)).ToList();

            foreach (var x in list)
            {
                NgoModel S = new NgoModel();
                S.ngoId = x.ngoId;
                S.name = x.name;
                S.emailId = x.emailId;
                S.password = x.password;
                S.panNo = x.panNo;
                S.acheivement = x.acheivement;
                S.uploadPan = x.uploadPan;
                S.parentOrg = x.parentOrg;
                S.websiteUrl = x.websiteUrl;
                S.createdOn = x.createdOn;
                S.createdBy = x.createdBy;
                S.updatedOn = x.updatedOn;
                S.updatedBy = x.updatedBy;
                S.deletedOn = x.deletedOn;
                S.deletedBy = x.createdBy;
                S.isActive = x.isActive;
                S.isApproved = x.isApproved;
                S.isDeleted = x.isDeleted;

                
                obj.Add(S);

            }

            nvm.listnm = obj;
            nvm.States = PopulateStatedd();
            return View("ReportNgo", nvm);
           

        }
    }



}