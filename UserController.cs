using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NgoCommunity.Models;


namespace NgoCommunity.Controllers.User
{
    public class UserController : Controller
    {
        NgoDBEntities db = new NgoDBEntities();

        // GET: User
        public ActionResult Signup(int id)
        {
            UserModel um = new UserModel();
            List<UserModel> obj = new List<UserModel>();

            um.roleId = id;

            obj.Add(um);
              
            return View("Signup",um);
        }

        [HttpPost]
        public ActionResult Edit(UserModel um)
        {

            if(um.roleId == 1)
            {
                if (um.userId == 0)
                {


                    if (db.UserMasters.Where(u => u.emailId == um.emailId)
                           .Count() > 0)
                    {
                        ModelState.AddModelError("CustomError", "This User Of the Organisation is Already exist.");
                        return View("Signup", um);
                       
                    }


                    if (db.UserMasters.Where(u => u.userName == um.userName)
                          .Count() > 0)
                    {
                        ModelState.AddModelError("CustomError", "This Ngo Of the Organisation is Already exist. Please Contact Admin ");
                        return View("Signup", um);

                    }




                    UserMaster tbl = new UserMaster();
                    tbl.userName = um.userName;
                    tbl.emailId = um.emailId;
                    tbl.password = um.password;
                    tbl.isActive = true;
                    tbl.isRegistered = false;
                    tbl.roleId = um.roleId;

                    db.UserMasters.Add(tbl);


                    NgoMaster tblng = new NgoMaster();
                    tblng.userId = tbl.userId;
                    tblng.name = tbl.userName;
                    tblng.emailId = tbl.emailId;
                    tblng.password = tbl.password;
                    tblng.isActive = true;
                    tblng.isApproved = false;
                    tblng.isDeleted = false;
                    tblng.createdOn = Convert.ToDateTime(DateTime.Now);
                    tblng.createdBy = tbl.userName;
                    db.NgoMasters.Add(tblng);


                    AddressMaster tblad = new AddressMaster();
                    tblng.addressId = tblad.addressId;
                    

                    db.AddressMasters.Add(tblad);

                    

                    db.SaveChanges();

                }
               
            }

            if (um.roleId == 2)
            {
                if (um.userId == 0)
                {

                    if (db.UserMasters.Where(u => u.emailId == um.emailId)
                      .Count() > 0)
                    {
                        ModelState.AddModelError("CustomError", "This User Of the Organisation is Already exist.");
                        return View("Signup", um);

                    }


                    UserMaster tbl = new UserMaster();
                    tbl.userName = um.userName;
                    tbl.emailId = um.emailId;
                    tbl.password = um.password;
                    tbl.isActive = true;
                    tbl.isRegistered = false;
                    tbl.roleId = um.roleId;

                    db.UserMasters.Add(tbl);

                    UserProfileMaster tblup = new UserProfileMaster();
                    tblup.firstName = tbl.userName;
                    tblup.userId = tbl.userId;
                    tblup.emailId = tbl.emailId;
                    tblup.password = tbl.password;
                    tblup.roleId = tbl.roleId;
                    tblup.createdOn = Convert.ToDateTime(DateTime.Now);
                    tblup.createdBy = tbl.userName;
                    tblup.isActive = true;
                   
                    db.UserProfileMasters.Add(tblup);


                    AddressMaster tblad = new AddressMaster();
                    tblup.addressId = tblad.addressId;


                    db.AddressMasters.Add(tblad);


                    db.SaveChanges();

                }

            }

            if (um.roleId == 3)
            {
                if (um.userId == 0)
                {

                    if (db.UserMasters.Where(u => u.emailId == um.emailId)
                      .Count() > 0)
                    {
                        ModelState.AddModelError("CustomError", "This User Of the Organisation is Already exist.");
                        return View("Signup", um);

                    }

                    UserMaster tbl = new UserMaster();
                    tbl.userName = um.userName;
                    tbl.emailId = um.emailId;
                    tbl.password = um.password;
                    tbl.isActive = true;
                    tbl.isRegistered = false;
                    tbl.roleId = um.roleId;

                    db.UserMasters.Add(tbl);

                    UserProfileMaster tblup = new UserProfileMaster();
                    tblup.firstName = tbl.userName;
                    tblup.userId = tbl.userId;
                    tblup.emailId = tbl.emailId;
                    tblup.password = tbl.password;
                    tblup.roleId = tbl.roleId;
                    tblup.createdOn = Convert.ToDateTime(DateTime.Now);
                    tblup.createdBy = tbl.userName;
                    tblup.isActive = true;
                    db.UserProfileMasters.Add(tblup);


                    AddressMaster tblad = new AddressMaster();
                    tblup.addressId = tblad.addressId;


                    db.AddressMasters.Add(tblad);


                    db.SaveChanges();

                }

            }


            ViewBag.message = "Thank You! You have Successifully registered!";

            return View("Login");
        }

        public ActionResult Login()
        {
            return View();
            
        }

        public ActionResult LoginPage()
        {
            return View();

        }




        public ActionResult CheckLogin(UserModel um)
        {
            var chkem = db.UserMasters.Where(u=>u.emailId == um.emailId).ToList();

            if (chkem.Count > 0)
            {
                var chkpass = db.UserMasters.Where(u=>u.password == um.password).ToList();

                if (chkpass.Count > 0)
                {

                    var p = db.UserMasters.Where(u => u.emailId == um.emailId && u.password == um.password && u.isActive == true).ToList();


                    if(p.Count > 0)

                    {
                        var x = db.UserMasters.Where(u => u.emailId == um.emailId && u.password == um.password).FirstOrDefault();

                        Session["EmailId"] = x.userName;
                        Session["UserId"] = x.userId;
                        Session["RoId"] = x.roleId;


                        if (x.roleId == 4)
                        {
                            return RedirectToAction("NgoRegistration", "Ngo");
                        }
                        if (x.roleId == 1)
                        {

                            var n = db.NgoMasters.Where(u => u.userId == x.userId).FirstOrDefault();
                            Session["NgoId"] = n.ngoId;

                            return RedirectToAction("UserNgoDetails", "Ngo");
                        }
                        if (x.roleId == 2 || x.roleId == 3)
                        {
                            var n = db.UserProfileMasters.Where(u => u.userId == x.userId).FirstOrDefault();
                            Session["ProfileId"] = n.profileId;
                            return RedirectToAction("UserProfileDetails", "UserProfile");
                        }
                    }
                       
                   
                }
            }


            ViewBag.message = "The Email Or Password you entered is incorrect. ";

            return View("Login");

        }



        public ActionResult Logout()
        {

            Response.Cache.SetCacheability(HttpCacheability.NoCache);

            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));

            Response.Cache.SetNoStore();



            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
          

            return View("Login");
        }


        public ActionResult Screen()
        {


            return View();
        }



        public ActionResult ForgetPassword(UserModel um)
        {
            var tbl = db.UserMasters.Where(u => u.emailId == um.emailId).ToList();

            if(tbl.Count == 1)
            {
                var x = db.UserMasters.Where(u => u.emailId == um.emailId).FirstOrDefault();

                x.emailId = um.emailId;
                x.password = um.password;


            }



            return View();
        }



        public ActionResult Forget(UserModel um)
        { 
                using (NgoDBEntities db = new NgoDBEntities())
                {
                   
                    var obj = db.UserMasters.Where(a => a.emailId == um.emailId && a.isActive==true).FirstOrDefault();
                    if (obj != null)
                    {
                        string value = generatePassword();


                      

                    var Body = "<h1> Ngo Community</h1><br/><h2><div style='color:green;'>Password Reset Sucessfully!! </div></h2><h3><p>Hello " + obj.userName + "</p></h3><br /><p>Your new Password is<u><b> " + value + "</b></u>";


                    Email Email = new Email
                        {
                            Sender = "brijmean@gmail.com",
                            Recipient = obj.emailId,
                            Subject = "Password Recovery",
                            Body = Body,
                        };
                        Email.Send();
                        obj.password = value;
                        db.SaveChanges();


                    ViewBag.message = "Password Reset Successfully!. Please check your Email";


                    }
                }
            
            return View("ForgetPassword");
        }
        public string generatePassword()
        {

            char[] arrPossibleChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
            int intPasswordLength = 8;
            string stringPassword = null;

            System.Random rand = new Random();

            int i = 0;
            for (i = 0; i < intPasswordLength; i++)
            {
                int intRandom = rand.Next(arrPossibleChars.Length);
                stringPassword = stringPassword + arrPossibleChars[intRandom].ToString();
            }

            return stringPassword;
        }



        public ActionResult ChangePassword()
        {

            return View();
        }


        public ActionResult ChangePass(UserModel um)
        {
            var x = db.UserMasters.Where(u=>u.emailId == um.emailId && u.password== um.CurrentPassword).ToList();

            if(x.Count == 1)
            {
                var y = db.UserMasters.Where(u => u.emailId == um.emailId && u.password == um.CurrentPassword).FirstOrDefault();
                y.password = um.password;
                db.SaveChanges();


                ViewBag.message = "Your Password has been changed Successfully!..";
            }



            return RedirectToAction("Login","User");
        }



        public JsonResult CheckEmail(string value)
        {
            
            var result = true;
            var user = db.UserMasters.Where(x => x.emailId == value).FirstOrDefault();

            if (user != null)
                result = false;

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
    }
}