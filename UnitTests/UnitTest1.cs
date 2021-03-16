using System;
using NUnit.Framework;
using StudyTrackr;
using StudyTrackr.Database;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {

        }

        [TestCase]
        public void User_Email_Valid()
        {
            var user = new User()
            {
                UserId = 12,
                StudentId = 2115,
                Password = "123ABC",
                StudentFirstName = "John",
                StudentLastName = "Doe",
                StudentEmail = "jdoe@email.com"
            };

            if (Validation.IsEmailValid(user.StudentEmail))
                Assert.Pass();
            else
                Assert.Fail();
        }

        [TestCase(".joe@email.com")]
        [TestCase("joe@email..@com")]
        [TestCase("123@..joe@email.com")]
        [TestCase(" ")]
        public void User_Email_Invalid(string emailAddress)
        {
            var user = new User()
            {
                UserId = 12,
                StudentId = 2115,
                Password = "123ABC",
                StudentFirstName = "John",
                StudentLastName = "Doe",
                StudentEmail = emailAddress
            };

            if (Validation.IsEmailValid(user.StudentEmail))
                Assert.Fail();
            else
                Assert.Pass();
        }

        [TestCase]
        public void Instructor_Phone_Number_Valid()
        {
            var course = new Course()
            {
                CourseId = 12,
                TermId = 5,
                UserId = 12,
                CourseNotification = false,
                CourseStatus = "Active",
                CourseStartDate = DateTime.Today,
                CourseEndDate = DateTime.Today.AddDays(30),
                InstructorName = "Tommy Joe",
                InstructorEmail = "tjoe@email.com",
                InstructorPhone = "5555555555"
            };

            if(Validation.IsPhoneNumberValid(course.InstructorPhone))
                Assert.Pass();
            else
                Assert.Fail();
        }

        [TestCase("PHONENUMBER")]
        [TestCase("123-BFD-A221")]
        [TestCase("1-800-FAB-LOUS")]
        [TestCase(" ")]
        public void Instructor_Phone_Number_Invalid(string phoneNumber)
        {
            var course = new Course()
            {
                CourseId = 12,
                TermId = 5,
                UserId = 12,
                CourseNotification = false,
                CourseStatus = "Active",
                CourseStartDate = DateTime.Today,
                CourseEndDate = DateTime.Today.AddDays(30),
                InstructorName = "Tommy Joe",
                InstructorEmail = "tjoe@email.com",
                InstructorPhone = phoneNumber
            };

            if (Validation.IsPhoneNumberValid(course.InstructorPhone))
                Assert.Fail();
            else
                Assert.Pass();
        }

        [TestCase]
        public void Course_Status_Not_Null_Or_Empty()
        {
            var course = new Course()
            {
                CourseId = 12,
                TermId = 5,
                UserId = 12,
                CourseNotification = false,
                CourseStatus = "Active",
                CourseStartDate = DateTime.Today,
                CourseEndDate = DateTime.Today.AddDays(30),
                InstructorName = "Tommy Joe",
                InstructorEmail = "tjoe@email.com",
                InstructorPhone = "5555555555"
            };

            if (Validation.IsFieldNull(course.CourseStatus))
                Assert.Pass();
            else
                Assert.Fail();
        }

        [TestCase]
        public void Course_Status_Null()
        {
            var course = new Course()
            {
                CourseId = 12,
                TermId = 5,
                UserId = 12,
                CourseNotification = false,
                CourseStatus = null,
                CourseStartDate = DateTime.Today,
                CourseEndDate = DateTime.Today.AddDays(30),
                InstructorName = "Tommy Joe",
                InstructorEmail = "tjoe@email.com",
                InstructorPhone = "5555555555"
            };

            if (Validation.IsFieldNull(course.CourseStatus))
                Assert.Fail();
            else
                Assert.Pass();
        }

        public void Course_Status_Empty()
        {
            var course = new Course()
            {
                CourseId = 12,
                TermId = 5,
                UserId = 12,
                CourseNotification = false,
                CourseStatus = "",
                CourseStartDate = DateTime.Today,
                CourseEndDate = DateTime.Today.AddDays(30),
                InstructorName = "Tommy Joe",
                InstructorEmail = "tjoe@email.com",
                InstructorPhone = "5555555555"
            };

            if (Validation.IsFieldNull(course.CourseStatus))
                Assert.Fail();
            else
                Assert.Pass();
        }
    }
}