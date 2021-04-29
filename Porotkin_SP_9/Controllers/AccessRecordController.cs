using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Porotkin_SP_9.Data;
using Porotkin_SP_9.Models;

namespace Porotkin_SP_9.Controllers
{
    public class AccessRecordController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AccessRecordController> _logger;
    
        public AccessRecordController(ApplicationDbContext db, ILogger<AccessRecordController> logger)
        {
            _db = db;
            _logger = logger;
        }


        public IActionResult Index()
        {
            IEnumerable<AccessRecord> objList = _db.AccessRecords;
            return View(objList);
        }

        // GET-Create
        public IActionResult Create()
        {
            return View();
        }

        // POST-Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AccessRecord obj)
        {
            _db.AccessRecords.Add(obj);
            _db.SaveChanges();
            _logger.LogInformation($"New record with id = {obj.Id} has been created.");
            return RedirectToAction("Index");
        }

        public IActionResult Update(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _db.AccessRecords.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(AccessRecord accessRecord)
        {
            _db.AccessRecords.Update(accessRecord);
            _db.SaveChanges();
            _logger.LogInformation($"Record with id = {accessRecord.Id} has been updated.");
            return RedirectToAction("Index");
        }

        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _db.AccessRecords.Find(id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(Guid id)
        {
            var editRecord = _db.AccessRecords.FirstOrDefault(element => element.Id == id);
            if (editRecord != null)
            {
                _db.AccessRecords.Remove(editRecord);
                _logger.LogInformation($"Record with id = {editRecord.Id} has been deleted.");
            }

            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult GetFile()
        {
            FileStream file = System.IO.File.Create("test.bin");
            BinaryWriter streamWriter = new BinaryWriter(file);
            streamWriter.Write(_db.AccessRecords.Count());
            foreach (var accessRecord in _db.AccessRecords)
            {
                streamWriter.Write(accessRecord.Id.ToString());
                streamWriter.Write(accessRecord.Login);
                streamWriter.Write(accessRecord.Passhash);
                streamWriter.Write(accessRecord.Email);
            }

            streamWriter.Close();
            _logger.LogInformation($"Binary file has been exported.");
            FileStream openedStream = System.IO.File.OpenRead("test.bin");
            return File(openedStream, "application/binary", "test.bin");
        }

        public IActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ImportFromFile(IFormFile file)
        {
            BinaryReader binaryReader = new BinaryReader(file.OpenReadStream());
            int count = binaryReader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                string id = binaryReader.ReadString();
                string login = binaryReader.ReadString();
                string passhash = binaryReader.ReadString();
                string email = binaryReader.ReadString();
                AccessRecord record = new AccessRecord
                {
                    Id = Guid.Parse(id), Login = login, Passhash = passhash, Email = email
                };
                _logger.LogInformation($"File with name = {file.FileName} has been read.");
                if (!_db.AccessRecords.Contains(record))
                {
                    _db.Add(record);
                }
            }

            binaryReader.Close();
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}