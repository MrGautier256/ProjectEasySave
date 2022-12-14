using CommonCode;

namespace TestUnitaires
{
    [TestClass]
    public class UTSaveService
    {
        [TestMethod]
        public void TestEncryptFile()
        {
            // Arrange
            const string contentText = "test Unitaire Encrypt";
            var tempFileInput = Path.GetTempFileName();
            var tempFileOutput = Path.GetTempFileName();
            var tempFileOutputOutput = Path.GetTempFileName();

            File.WriteAllText(tempFileInput, contentText);

            // Act
            SaveService.EncryptFile(tempFileInput, tempFileOutput);
            SaveService.EncryptFile(tempFileOutput, tempFileOutputOutput);

            // Assert
            var OutputContent = File.ReadAllLines(tempFileOutput);
            var OutputOutputContent = File.ReadAllLines(tempFileOutputOutput);
            FileInfo fiTempFileInput = new FileInfo(tempFileInput);
            FileInfo fiTempFileOutputOutput = new FileInfo(tempFileOutputOutput);


            Assert.AreEqual(1, OutputOutputContent.Length);
            Assert.AreEqual(1, OutputContent.Length);
            Assert.AreEqual(fiTempFileInput.Length, fiTempFileOutputOutput.Length);
            Assert.AreNotEqual(contentText, OutputContent[0]);
            Assert.AreEqual(contentText, OutputOutputContent[0]);
        }
    }
}