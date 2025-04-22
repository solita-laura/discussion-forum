import { useEffect, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import TopicMessage from "../components/TopicMessage";
import MessageForm from "../components/MessageForm";


function TopicScreen() {

    const navigate = useNavigate();
    const location = useLocation();

    const { topicId, topicname } = location.state ?? {};

    type Message = {
      id: number;
      content: string;
      upvotes: number;
      postdate: Date;
      userid: number;
    }

    type Error = {
      errorMessage: string;
    }

    type messageContent = {
      content: string;
    }

    type updateMessage = {
      updatedcontent: string;
    }

    const [messages, setMessages] = useState<Message[]>([]);
    const [error, setError] = useState<Error>({errorMessage: ''});
    const [messageContent, setMessageContent] = useState<messageContent>({content: ''});
    const [updateMessage, setUpdateMessage] = useState<updateMessage>({updatedcontent: ''});

    const getMessages = async () => {
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
            switch (response.status) {
              case 401:
                navigate('/login');
                break;
              default: 
                setMessages([]);
                break; 
            }
          }
      });
    };

    const addMessage = async (event: React.FormEvent<HTMLFormElement>) => {
      event.preventDefault();

      if(!messageContent.content) {
        return;
      }
      
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
            setMessageContent({content: ''});
            getMessages();
          } else {
              setError({ errorMessage: "Error adding message" });
            }
        }
      );
    };

    const sendMessageUpdate = async (event: React.FormEvent<HTMLFormElement>, messageid: number) => {
      
      event.preventDefault();

      if(!updateMessage.updatedcontent) {
        return;
      }
      
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
        }
      );
    } 


    const handleChange = (event: React.ChangeEvent<HTMLTextAreaElement>) => {
      setMessageContent({
        ...messageContent,
        [event.target.name]: event.target.value
      });
    }

    const updateMessageContent = async (event: React.ChangeEvent<HTMLTextAreaElement>) => {
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
      <h1 className="top-0 uppercase text-cyan-900 p-2 w-full">{topicname}</h1>
      {messages ? messages.map((message) => (
        <div key={message.id} onLoad={() => setUpdateMessage({updatedcontent: message.content})}>
          <div>
            <TopicMessage 
              content={message.content}
              upvotes={message.upvotes}
              postdate = {new Date(message.postdate)}
              userid = {message.userid}
              messageid={message.id}
              sendUpdatedMessage={sendMessageUpdate}
              updateMessageContent={updateMessageContent}
              updateContent={updateMessage.updatedcontent}
              setInitialContent={setUpdateMessage}
              />
          </div>
        </div>
      )) : <h1>No messages found</h1>}
      <div>
          <MessageForm
            message={messageContent.content}
            addMessage={handleChange}
            sendMessage={addMessage}
            error = {error.errorMessage}
          />
      </div>
    </div>
  )
}

export default TopicScreen;