using System;
using Xunit;
using cs330_proj1;
using System.Collections.Generic;
using Moq;
using System.Linq;
using System.Reflection;

namespace CourseProject.Tests;

public class CourseServicesTests
{
        [Fact]
        public void GetOfferingsByGoalIdAndSemester_GoalNotFound_ExceptionThrown()
        {
            // Arrange
            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(GetTestCourses());
            mockRepository.Setup(m => m.Goals).Returns(new List<CoreGoal>(){
            new CoreGoal() {
                Courses = GetTestCourses(),
                Description = "test",
                Id = "CG1",
                Name = "English Literacy"
            }
            });

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                new CourseOffering() {
                    Section = "1",
                    Semester = "Spring 2021",
                    TheCourse = GetTestCourses().First()
                }
            });

            var courseServices = new CourseServices(mockRepository.Object);
            var goalId = "CG5";
            var semester = "Spring 2021";

            // Act/Assert
            Assert.Throws<Exception>(() => courseServices.GetOfferingsByGoalIdAndSemester(goalId, semester));
        }


        [Fact]
        public void GetOfferingsByGoalIdAndSemester_GoalIsFoundAndOneCourseOfferingIsInSemester_OfferingIsReturned()
        {
            // Arrange
            var course = new Course() {
                Name= "ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr"
            };

            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course> {
                course});

            mockRepository.Setup(m => m.Goals).Returns(new List<CoreGoal>(){
            new CoreGoal() {
                Courses = GetTestCourses(),
                Description = "test",
                Id = "CG1",
                Name = "English Literacy"
            }
            });

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>() {
                new CourseOffering() {
                    Section = "1",
                    Semester = "Spring 2021",
                    TheCourse = course
                }
            });

            
            var goalId = "CG1";
            var semester = "Spring 2021";
            var courseServices = new CourseServices(mockRepository.Object);

            //Act
            var result = courseServices.GetOfferingsByGoalIdAndSemester(goalId, semester);

            // Assert
            var itemInList = Assert.Single(result);
            // Assert.Equal(2, result.Count());
            Assert.Equal(semester, itemInList.Semester);
            Assert.Equal(course.Name, itemInList.TheCourse.Name);
            
           
        }

        //Add unit tests for GetOfferingsByGoalIdAndSemester_GoalIsFoundAndMultipleCourseOfferingsAreInSemester_OfferingsAreReturned()
        // Add unit test for GetOfferingsByGoalIdAndSemester_GoalIsFoundAndNoCourseOfferingIsInSemester_EmptyListIsReturned()


        //user story 2
        [Fact]

        public void GetAllCourses_CoursesExist_ReturnsCourses()
        {
            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(GetTestCourses());

            var service = new CourseServices(mockRepository.Object);

            var result = service.getCourses();

            Assert.Equal(2, result.Count());
        }

        [Fact]

        public void GetAllCourses_NoCourses_ReturnsEmptyList()
        {
            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(new List<Course>());

            var service = new CourseServices(mockRepository.Object);

            var result = service.getCourses();

            Assert.Empty(result);
        }


        //user story 3
        [Fact]
        public void getCourseOfferingsBySemester_SemesterMatches_ReturnsMatches()
        {
            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(GetTestCourses());

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>()
            {
                new CourseOffering(){Semester="Spring 2021", TheCourse=GetTestCourses().First()},
                new CourseOffering(){Semester="Fall 2021", TheCourse=GetTestCourses().Last()}
            });

            var service = new CourseServices(mockRepository.Object);

            var result = service.getCourseOfferingsBySemester("Spring 2021");

            Assert.Single(result);
            Assert.Equal("Spring 2021", result.First().Semester);
        }

        [Fact]
        public void getCourseOfferingsBySemester_NoMatches_ReturnsNoMatches()
        {
            var mockRepository = new Mock<ICourseRepository>();
            mockRepository.Setup(m => m.Courses).Returns(GetTestCourses());

            mockRepository.Setup(m => m.Offerings).Returns(new List<CourseOffering>());
       

            var service = new CourseServices(mockRepository.Object);

            var result = service.getCourseOfferingsBySemester("Winter 2030");

            Assert.Empty(result);
        }
        private List<Course> GetTestCourses()
        {
            return new List<Course>(){
            new Course() {
                Name="ARTD 201",
                Title="graphic design",
                Credits=3.0,
                Description="graphic design descr"

            },
            new Course() {
                Name="ARTS 101",
                Title="art studio",
                Credits=3.0,
                Description="studio descr"

            }
            };
        }
}
