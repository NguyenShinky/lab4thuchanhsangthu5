using lab4thuchanhsangthu5.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lab4thuchanhsangthu5.Controllers
{
    public class CoursesController : Controller
    {
        // GET: Courses
        public ActionResult Create()
        {
            BigSchoolContext context = new BigSchoolContext();
            Course objCourse = new Course();
            objCourse.ListCategory = context.Category.ToList();

            return View(objCourse);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Course objCourse) 
        {
            BigSchoolContext context = new BigSchoolContext();

            ModelState.Remove("LecturerId");
            if (!ModelState.IsValid)
            {
                objCourse.ListCategory = context.Category.ToList();
                return View("Create", objCourse);
            }

            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            objCourse.LecturerId = user.Id;

            if (ModelState.IsValid)
            {
                context.Course.Add(objCourse);
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }


            ViewBag.IdCategory = new SelectList(context.Category , "Id", "Name", objCourse.CategoryId);
            return View(objCourse);
        }


        public ActionResult Attending()
        {
            BigSchoolContext context = new BigSchoolContext();
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            var listAttendances = context.Attendance.Where(p => p.Attendee == currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (Attendance temp in listAttendances)
            { 
                Course objCourse = temp.Course;
                objCourse.LecturerId = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                    .FindById(objCourse.LecturerId).Name;
                courses.Add(objCourse);
            }
            return View(courses);
        }


        public ActionResult Mine() 
        {
            ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
             .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            var course = context.Course.Where(c => c.LecturerId == currentUser.Id && c.DateTime > DateTime.Now).ToList();
            foreach (Course i in course) 
            {
                i.LectureNAme = currentUser.Name;
            }
            return View(course);
        }

        //public ActionResult Edit(int id)
        //{
        //    BigSchoolContext context = new BigSchoolContext();
        //    var userID = User.Identity.GetUserId();
        //    var course = context.Course.Single(c => c.Id == id && c.LecturerId == userID);
        //    var viewModel = new CourseViewModel
        //    {
        //        categories = context.Category.ToList(),
        //        Date = course.DateTime.ToString("dd/MM/yyyy"),
        //        Time = course.DateTime.ToString("HH:mm"),
        //        Category = course.CategoryId,
        //        Place = course.Place
        //    };
        //    return View("Create", viewModel);
        //}
        [Authorize]
        public ActionResult Edit(int id)
        {
            BigSchoolContext context = new BigSchoolContext();
            Course course = context.Course.SingleOrDefault(p => p.Id == id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }

        [Authorize]
        [HttpPost]

        public ActionResult Edit(Course course)
        {

            BigSchoolContext context = new BigSchoolContext();
            Course courseupdate = context.Course.SingleOrDefault(p => p.Id == course.Id);
            if (courseupdate != null)
            {
                context.Course.AddOrUpdate(course);
                context.SaveChanges();
            }

            return RedirectToAction("ListCoure");

        }


        public ActionResult Delete(int Id)
        {
            using (BigSchoolContext context = new BigSchoolContext())
            {
                return View(context.Course.Where(p => p.Id == Id).FirstOrDefault());
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult Delete(int Id, FormCollection collection)
        {
            try
            {
                using (BigSchoolContext context = new BigSchoolContext())
                {
                    Course course = context.Course.Where(p => p.Id == Id).FirstOrDefault();
                    context.Course.Remove(course);
                    context.SaveChanges();
                }
                return RedirectToAction("ListBook");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult LectureIamGoing()
        {
            ApplicationUser currentUser =
           System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
            .FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
            BigSchoolContext context = new BigSchoolContext();
            //danh sách giảng viên được theo dõi bởi người dùng (đăng nhập) hiện tại
            var listFollwee = context.Following.Where(p => p.FollowerId ==
            currentUser.Id).ToList();
            //danh sách các khóa học mà người dùng đã đăng ký
            var listAttendances = context.Attendance.Where(p => p.Attendee ==
            currentUser.Id).ToList();
            var courses = new List<Course>();
            foreach (var course in listAttendances)
            {
                foreach (var item in listFollwee)
                {
                    if (item.FolloweeId == course.Course.LecturerId)
                    {
                        Course objCourse = course.Course;
                        objCourse.LectureNAme =
                       System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>()
                        .FindById(objCourse.LecturerId).Name;
                        courses.Add(objCourse);
                    }
                }

            }
            return View(courses);
        }
    }
        
    }
