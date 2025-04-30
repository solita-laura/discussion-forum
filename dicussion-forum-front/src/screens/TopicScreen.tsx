import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import TopicMessage from "../components/TopicMessage";
import MessageForm from "../components/MessageForm";


/**
 * TopicScreen shows all the messages in a topic
 */
function TopicScreen() {

    const navigate = useNavigate();

    //retrieve the topicId and topicname from the location state (passed from the Dashboard)
    const location = useLocation();
    const { topicId, topicname } = location.state ?? {};

    type Message = {
      id: number;
      content: string;
      upvotes: number;
      postdate: Date;
      userid: string;
      username: string;
    }

    type Error = {
      errorMessage: string;
    }

    type MessageContent = {
      content: string;
    }

    type UpdateMessage = {
      updatedcontent: string;
    }

    const [messages, setMessages] = useState<Message[]>([]);
    const [error, setError] = useState<Error>({errorMessage: ''});
    const [messageContent, setMessageContent] = useState<MessageContent>({content: ''});
    const [updateMessage, setUpdateMessage] = useState<UpdateMessage>({updatedcontent: ''});

    
    /**
     * Fetches all the messages from the API
     */

    const getMessages = async () => {

      try{
        await fetch('http://localhost:5055/api/Topics/Message?topicid=' + topicId, {
          method: 'GET',
          credentials: 'include',
          headers: {
            'Content-Type': 'application/json',
          },
        })
        .then(async response => {
          if (response.ok) {
            setMessages(await response.json());
          } else {
            setError({ errorMessage: "Error fetching messages" });
            setMessages([]);
            navigate('/login');
          }
        });
      } catch {
        setError({ errorMessage: "Error fetching messages" });
      }

    };

    /**
     * Adds a message to the topic
     * @param event FormEvent
     */

    const addMessage = async (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();

      //dont send emtpy messages
      if(!messageContent.content) {
        return;
      }
      
      try{
        await fetch('http://localhost:5055/api/Topics/Message', {
          method: 'POST',
          credentials: 'include',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            topicid: topicId,
            content: messageContent.content,
          }),
        })
        .then(async response => {
          if (response.ok) {
            setError({errorMessage: ''});
            setMessageContent({content: ''});
            getMessages();
          } else {
              setError({ errorMessage: "Error adding message" });
            }
        });
      } catch {
        setError({ errorMessage: "Error adding message" });
      }

    };

    /**
     * Updates a message
     * @param event FormEvent
     * @param messageid number
     */

    const sendMessageUpdate = async (event: React.FormEvent<HTMLFormElement>, messageid: number) => {
      
      event.preventDefault();

      //dont send empty message
      if(!updateMessage.updatedcontent) {
        return;
      }
      
      try{
        await fetch('http://localhost:5055/api/Topics/Message?messageid=' + messageid, {
          method: 'PUT',
          credentials: 'include',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify(updateMessage.updatedcontent),
        })
        .then(async response => {
          if (response.ok) {
            setError({errorMessage: ''});
            setUpdateMessage({updatedcontent: ''});
            getMessages();
          } else {
              setError({ errorMessage: "Error updating message" });
            }
        });
      } catch {
        setError({ errorMessage: "Error updating message" });
      }

    } 

    /**
     * set the message content on change
     * @param event ChangeEvent
     */
    const handleMessageChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
      setMessageContent({
        ...messageContent,
        [event.target.name]: event.target.value
      });
    }

    /**
     * set the updated message content on change
     * @param event ChangeEvent
     */
    const handleUpdatedContentChange = async (event: React.ChangeEvent<HTMLTextAreaElement>) => {
      setUpdateMessage({
        ...updateMessage,
        [event.target.name]: event.target.value
      })
    }

    useEffect(() => {
      getMessages();
    }, []);

  return (
    <div className="flex flex-col justify-center">
      <h1 className="top-0 uppercase text-cyan-900 p-5 w-full">{topicname}</h1>
      {messages ? messages.map((message) => (
        <div key={message.id}>
          <div className="border-2 m-3">
            <TopicMessage 
              content={message.content}
              upvotes={message.upvotes}
              postdate = {new Date(message.postdate)}
              userid = {message.userid}
              messageid={message.id}
              sendUpdatedMessage={sendMessageUpdate} //send the message to the API
              updateMessageContent={handleUpdatedContentChange} //listen to changes
              updateContent={updateMessage.updatedcontent} //updated content
              setInitialContent={setUpdateMessage} //set initial content as the current message content
              username={message.username}
              />
          </div>
        </div>
      )) : <h1>No messages found</h1>}
      <div className="w-4xl">
          <MessageForm
            message={messageContent.content}
            addMessage={handleMessageChange}
            sendMessage={addMessage}
            error = {error.errorMessage}
          />
      </div>
    </div>
  )
}

export default TopicScreen;