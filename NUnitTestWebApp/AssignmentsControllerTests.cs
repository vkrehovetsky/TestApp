using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TestWebApp.Controllers;
using TestWebApp.Models;

namespace Tests
{
    public class Tests
    {
        private Mock<IRepository> _mock;
        private AssignmentsController _controller;
        private List<Assignment> _assignments;

        [SetUp]
        public void Setup()
        {
            _mock = new Mock<IRepository>();
            _controller = new AssignmentsController(_mock.Object);
            _assignments = new List<Assignment>
            {
                new Assignment { Id = 1, Name = "Task1" },
                new Assignment { Id = 2, Name = "Task2" },
                new Assignment { Id = 3, Name = "Task3" },
                new Assignment { Id = 4, Name = "Task4" }
            };
        }

        [Test]
        public void GetAssignmensReturnstWithAListOfAssignments()
        {
            // Arrange
            _mock.Setup(repo => repo.GetAll()).Returns(GetTestTasks());

            // Act
            var result = _controller.Get();

            // Assert
            Assert.IsAssignableFrom<List<Assignment>>(result);
            Assert.AreEqual(GetTestTasks().Count, result.Count());
        }

        [Test]
        public void GetAssignmenReturnsNotFoundResultWhenIdIsZero()
        {
            // Act
            var result = _controller.Get(0);

            // Arrange
            Assert.IsAssignableFrom<NotFoundResult>(result);
        }

        [Test]
        public void GetAssignmentReturnsAssignmentWhenIdIsOne()
        {
            // Arrange
            int id = 1;
            _mock.Setup(repo => repo.Get(id)).Returns(GetTestTasks().FirstOrDefault(p => p.Id == id));

            // Act
            IActionResult result = _controller.Get(id);
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var item = okObjectResult.Value as Assignment;

            // Assert
            Assert.IsNotNull(item);
            Assert.IsAssignableFrom<Assignment>(item);
            Assert.AreEqual("Task1", item.Name);
            Assert.AreEqual(id, item.Id);
        }

        [Test]
        public void AddAssignmenReturnsBadRequestResultWhenAssignmenIsNull()
        {
            // Act
            var result = _controller.Post(null);

            // Arrange
            Assert.IsAssignableFrom<BadRequestResult>(result);
        }

        [Test]
        public void AddAssignmenReturnsAssignmentFromTheEnd()
        {
            // Arrange
            Assignment assignment = new Assignment { Id = 6, Name = "Task6" };
            _assignments.Add(assignment);
            _mock.Setup(repo => repo.Add(assignment)).Returns(GetTestTasks().FirstOrDefault(p => p.Id == 6));

            // Act
            var result = _controller.Post(assignment);
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var item = okObjectResult.Value as Assignment;

            // Assert
            Assert.IsNotNull(okObjectResult);
            Assert.IsAssignableFrom<Assignment>(item);
            Assert.AreEqual(assignment.Name, item.Name);
            Assert.AreEqual(assignment.Id, item.Id);
        }

        [Test]
        public void SetPriorityForTheCurrentAssignment()
        {
            // Arrange
            int id = 1;
            int priority = 3;
            _mock.Setup(repo => repo.SetPriority(id, priority)).Returns(GetTestTasks().FirstOrDefault(p => p.Id == 3));

            // Act
            IActionResult result = _controller.SetPriority(id, priority);
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var item = okObjectResult.Value as Assignment;

            // Assert
            Assert.IsNotNull(okObjectResult);
            Assert.IsAssignableFrom<Assignment>(item);
            Assert.AreEqual("Task3", item.Name);
            Assert.AreEqual(3, item.Id);
        }

        [Test]
        public void UpPriorityForTheCurrentAssignment()
        {
            // Arrange
            int id = 3;
            _mock.Setup(repo => repo.Up(id)).Returns(GetTestTasks().FirstOrDefault(p => p.Id == id));

            // Act
            IActionResult result = _controller.Up(id);
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var item = okObjectResult.Value as Assignment;

            // Assert
            Assert.IsNotNull(okObjectResult);
            Assert.IsAssignableFrom<Assignment>(item);
            Assert.AreEqual("Task3", item.Name);
            Assert.AreEqual(id, item.Id);
        }

        [Test]
        public void DownPriorityForTheCurrentAssignment()
        {
            // Arrange
            int id = 2;
            _mock.Setup(repo => repo.Down(id)).Returns(GetTestTasks().FirstOrDefault(p => p.Id == id));

            // Act
            IActionResult result = _controller.Down(id);
            var okObjectResult = result as OkObjectResult;
            Assert.IsNotNull(okObjectResult);
            var item = okObjectResult.Value as Assignment;

            // Assert
            Assert.IsNotNull(okObjectResult);
            Assert.IsAssignableFrom<Assignment>(item);
            Assert.AreEqual("Task2", item.Name);
            Assert.AreEqual(id, item.Id);
        }

        private List<Assignment> GetTestTasks()
        {
            return _assignments;
        }
    }
}