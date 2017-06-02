using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONConsumer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using JsonReader = JSONConsumer.JsonReader;

namespace JSONConsumerTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestReceiveJson()
        {
            string url = "http://agl-developer-test.azurewebsites.net/people.json";            
            var result = JsonReader.GetJson(url);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestReceiveJsonInvalidUrl()
        {
            string url = "BORK";
            var result = JsonReader.GetJson(url);
            Assert.IsNotNull(result.Exception);
            Assert.AreEqual(TaskStatus.Faulted,result.Status);
            Assert.IsTrue(result.Exception.InnerException is InvalidOperationException);
        }

        [TestMethod]
        public void TestDeSerializeJson()
        {
            string testinput =
                "[{\"name\":\"Bob\",\"gender\":\"Male\",\"age\":23,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"},{\"name\":\"Fido\",\"type\":\"Dog\"}]},{\"name\":\"Jennifer\",\"gender\":\"Female\",\"age\":18,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"}]},{\"name\":\"Steve\",\"gender\":\"Male\",\"age\":45,\"pets\":null},{\"name\":\"Fred\",\"gender\":\"Male\",\"age\":40,\"pets\":[{\"name\":\"Tom\",\"type\":\"Cat\"},{\"name\":\"Max\",\"type\":\"Cat\"},{\"name\":\"Sam\",\"type\":\"Dog\"},{\"name\":\"Jim\",\"type\":\"Cat\"}]},{\"name\":\"Samantha\",\"gender\":\"Female\",\"age\":40,\"pets\":[{\"name\":\"Tabby\",\"type\":\"Cat\"}]},{\"name\":\"Alice\",\"gender\":\"Female\",\"age\":64,\"pets\":[{\"name\":\"Simba\",\"type\":\"Cat\"},{\"name\":\"Nemo\",\"type\":\"Fish\"}]}]";
            var result = JsonReader.DeserializeJsonToPersonRecords(testinput);
            var pets = result.Where(owner=>owner.Pets!=null).SelectMany(owner => owner.Pets).ToList();

            Assert.AreEqual(6,result.Count);
            Assert.AreEqual(10,pets.Count);
        }

        [TestMethod]
        public void TestFilterRecordsDogCount()
        {
            string testinput =
                "[{\"name\":\"Bob\",\"gender\":\"Male\",\"age\":23,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"},{\"name\":\"Fido\",\"type\":\"Dog\"}]},{\"name\":\"Jennifer\",\"gender\":\"Female\",\"age\":18,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"}]},{\"name\":\"Steve\",\"gender\":\"Male\",\"age\":45,\"pets\":null},{\"name\":\"Fred\",\"gender\":\"Male\",\"age\":40,\"pets\":[{\"name\":\"Tom\",\"type\":\"Cat\"},{\"name\":\"Max\",\"type\":\"Cat\"},{\"name\":\"Sam\",\"type\":\"Dog\"},{\"name\":\"Jim\",\"type\":\"Cat\"}]},{\"name\":\"Samantha\",\"gender\":\"Female\",\"age\":40,\"pets\":[{\"name\":\"Tabby\",\"type\":\"Cat\"}]},{\"name\":\"Alice\",\"gender\":\"Female\",\"age\":64,\"pets\":[{\"name\":\"Simba\",\"type\":\"Cat\"},{\"name\":\"Nemo\",\"type\":\"Fish\"}]}]";
            var result = JsonReader.DeserializeJsonToPersonRecords(testinput);
            var processedData = PetProcessor.ProcessPetOwners(result, "DOG");

            Assert.AreEqual(1,processedData.Count);
            Assert.IsNotNull(processedData["Male"]);
            Assert.AreEqual(2, processedData["Male"].Count);
        }


        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestFilterRecordsFemaleDoesntExist()
        {
            string testinput =
                "[{\"name\":\"Bob\",\"gender\":\"Male\",\"age\":23,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"},{\"name\":\"Fido\",\"type\":\"Dog\"}]},{\"name\":\"Jennifer\",\"gender\":\"Female\",\"age\":18,\"pets\":[{\"name\":\"Garfield\",\"type\":\"Cat\"}]},{\"name\":\"Steve\",\"gender\":\"Male\",\"age\":45,\"pets\":null},{\"name\":\"Fred\",\"gender\":\"Male\",\"age\":40,\"pets\":[{\"name\":\"Tom\",\"type\":\"Cat\"},{\"name\":\"Max\",\"type\":\"Cat\"},{\"name\":\"Sam\",\"type\":\"Dog\"},{\"name\":\"Jim\",\"type\":\"Cat\"}]},{\"name\":\"Samantha\",\"gender\":\"Female\",\"age\":40,\"pets\":[{\"name\":\"Tabby\",\"type\":\"Cat\"}]},{\"name\":\"Alice\",\"gender\":\"Female\",\"age\":64,\"pets\":[{\"name\":\"Simba\",\"type\":\"Cat\"},{\"name\":\"Nemo\",\"type\":\"Fish\"}]}]";
            var result = JsonReader.DeserializeJsonToPersonRecords(testinput);
            var processedData = PetProcessor.ProcessPetOwners(result, "DOG");
            var fail = processedData["Female"];
        }
    }
}
