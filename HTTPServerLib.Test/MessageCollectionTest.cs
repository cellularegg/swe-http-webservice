using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HTTPServerLib.Test
{
    public class MessageCollectionTest
    {
        [Test]
        public void TestMessageCollectionIndex()
        {
            // Arrange
            MessageCollection msgColl = new MessageCollection();
            int addingTillIdx = 3;
            for (int i = 0; i < addingTillIdx; i++)
            {
                msgColl.AddMessage("Test");
            }
            // Act
            int actualIdx = msgColl.MaxIdx;
            // Assert
            Assert.AreEqual(addingTillIdx, actualIdx);
        }
        [Test]
        public void TestMessageCollectionRemove()
        {
            // Arrange
            MessageCollection msgColl = new MessageCollection();
            msgColl.AddMessage("Test");
            msgColl.AddMessage("Test");
            msgColl.AddMessage("Test");
            msgColl.AddMessage("Test");
            // Act
            bool actualDeleteIdx100 = msgColl.DeleteMessage(100);
            bool actualDeleteIdx0 = msgColl.DeleteMessage(0);
            bool actualDeleteIdx0_2 = msgColl.DeleteMessage(0);
            bool actualDeleteIdx3 = msgColl.DeleteMessage(3);

            // Assert
            Assert.IsFalse(actualDeleteIdx100);
            Assert.IsTrue(actualDeleteIdx0);
            Assert.IsFalse(actualDeleteIdx0_2);
            Assert.IsTrue(actualDeleteIdx3);
        }
    }
}
