using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace HTTPServerLib.Test
{
    public class MessageCollectionTest
    {
        [Test]
        public void TestMessageCollectionIndex()
        {
            // Arrange
            MessageCollection msgColl = MessageCollection.GetMessageCollection();
            int addingTillIdx = 3;
            for (int i = 0; i < addingTillIdx; i++)
            {
                msgColl.AddMessage("Test");
            }
            // Act
            int actualIdx = msgColl.MaxIdx;
            // Assert
            Assert.AreEqual(addingTillIdx, actualIdx);
            msgColl.Reset();
        }
        [Test]
        public void TestMessageCollectionRemove()
        {
            // Arrange
            MessageCollection msgColl = MessageCollection.GetMessageCollection();
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
            msgColl.Reset();
        }

        [Test]
        public void TestMessageCollectionGetMessageAsJson()
        {
            // Arrange
            MessageCollection msgColl = MessageCollection.GetMessageCollection();
            // Id: 0
            string msg0Content = "Hey, This is just a test Message!";
            msgColl.AddMessage(msg0Content);
            // Id: 1
            string msg1Content = "Another test Message!";
            msgColl.AddMessage(msg1Content);
            // Act
            string actualJsonStringMsg0 = msgColl.GetMessageAsJson(0);
            Tuple<int, string> message0 = msgColl.GetMsgTupleFromJson(actualJsonStringMsg0);
            string actualJsonStringMsg1 = msgColl.GetMessageAsJson(1);
            Tuple<int, string> message1 = msgColl.GetMsgTupleFromJson(actualJsonStringMsg1);
            string actualJsonStringMsg100 = msgColl.GetMessageAsJson(100);
            // Assert
            Assert.AreEqual(0, message0.Item1);
            Assert.AreEqual(msg0Content, message0.Item2);
            Assert.AreEqual(1, message1.Item1);
            Assert.AreEqual(msg1Content, message1.Item2);
            Assert.IsEmpty(actualJsonStringMsg100);
            msgColl.Reset();
        }

        [Test]
        public void TestMessageCollectionGetMsgTupleFromJson()
        {
            // Arrange
            MessageCollection msgColl = MessageCollection.GetMessageCollection();
            int msg0Id = 0;
            string msg0Content = "This is just a sample message.";
            string jsonMsg0 = "{ \"Id\": " + msg0Id + ", \"Content\": \"" + msg0Content + "\" }";
            string jsonMsg1 = "{ \"Content\": \"This is just a sample message.\" }";
            string jsonMsg2 = " \"Id\":0, \"Content\": \"This is just a sample message.\" ";
            string jsonMsg3 = "";

            // Act
            Tuple<int, string> msg0 = msgColl.GetMsgTupleFromJson(jsonMsg0);
            Tuple<int, string> msg1 = msgColl.GetMsgTupleFromJson(jsonMsg1);
            Tuple<int, string> msg2 = msgColl.GetMsgTupleFromJson(jsonMsg2);
            Tuple<int, string> msg3 = msgColl.GetMsgTupleFromJson(jsonMsg3);

            // Assert
            Assert.AreEqual(msg0.Item1, msg0Id);
            Assert.AreEqual(msg0.Item2, msg0Content);
            Assert.IsNull(msg1);
            Assert.IsNull(msg2);
            Assert.IsNull(msg3);
            msgColl.Reset();
        }

        [Test]
        public void TestMessageCollectionUpdateMessage()
        {
            // Arrange
            MessageCollection msgColl = MessageCollection.GetMessageCollection();
            msgColl.AddMessage("Hey");
            string updatedContent = "Hey, This is my updated Message Content";
            // Act
            bool actualHasSucceeded = msgColl.UpdateMessage(0, updatedContent);
            string actualMsgContent = msgColl.GetMessageContent(0);
            bool shouldFail = msgColl.UpdateMessage(1, "Test");
            // Assert
            Assert.AreEqual(actualMsgContent, updatedContent);
            Assert.IsTrue(actualHasSucceeded);
            Assert.IsFalse(shouldFail);
            msgColl.Reset();
        }

        [Test]
        public void TestMessageCollectionGetMessageContent()
        {
            // Arrange
            MessageCollection msgColl = MessageCollection.GetMessageCollection();
            string msgContent = "Sample Message";
            msgColl.AddMessage(msgContent);
            // Act
            string actualMsgContent = msgColl.GetMessageContent(0);
            string actualEmptyMsgContent = msgColl.GetMessageContent(1);
            // Assert
            Assert.AreEqual(msgContent, actualMsgContent);
            Assert.IsEmpty(actualEmptyMsgContent);
            msgColl.Reset();
        }
    }
}
