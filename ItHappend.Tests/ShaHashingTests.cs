using NUnit.Framework;
using ItHappened.Infrastructure;

namespace ItHappend.Tests
{
    public class ShaHashingTests
    {
        private ShaHashing _sha = new ShaHashing();        
        
        [Test]
        public void VerifyPassword_ForCorrectPassword_ReturnTrue()
        {
            
            var password = "qwerty";
            var hashedPassword = _sha.HashPassword(password);
            var expectedResult = true;

            var result = _sha.Verify(password, hashedPassword);
           
            Assert.AreEqual(result,expectedResult);
        }

        [Test]
        public void VerifyPassword_ForIncorrectPassword_ReturnFalse()
        {
            
            var password = "qwerty";
            var hashedPassword = _sha.HashPassword(password);
            var expectedResult = false;

            var result = _sha.Verify("123", hashedPassword);
           
            Assert.AreEqual(result,expectedResult);
        }
    }
}