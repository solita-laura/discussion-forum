import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import EditIcon from '@mui/icons-material/Edit';
import React, { useEffect, useState } from 'react';
import { GetUserId } from '../functions/GetUserId';


type MessageProps = {
  content: string;
  upvotes: number;
  postdate: Date;
  userid: string;
  messageid: number;
  updateContent:string;
  username: string;
  sendUpdatedMessage: (event: React.FormEvent<HTMLFormElement>, messageid: number) => void;
  updateMessageContent: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
  setInitialContent: React.Dispatch<React.SetStateAction<{updatedcontent: string}>>;
};

/**
 * Display the message in the topic
 * @param props Props for the message
 * @returns TopicMessage component
 */
function TopicMessage (props: MessageProps) {

  const [isEditing, setIsEditing] = useState(false);
  const [sessionUserId, setSessionUserId] = useState<string | null>(null);

  /**
   * stop further propagations of the event and toggle the edit mode 
   * and set the current content as base before editing the message
   * @param event MouseEvent
   */
  const handleEditClick = (event: React.MouseEvent) => {
    event.stopPropagation();
    props.setInitialContent({updatedcontent: props.content});
    setIsEditing(!isEditing);
  }

  /**
   * Submit the updated message
   * @param event FormEvent
   */
  const handleSubmitMessage = (event: React.FormEvent<HTMLFormElement>) => {
    props.sendUpdatedMessage(event, props.messageid);
    setIsEditing(false);
  };

  /**
   * Get the user id from the server
   */
  useEffect(() => {
    try{
    async function fetchUserId() {
      const id = await GetUserId();
      if (id) {
        setSessionUserId(id);
      } else {
        setSessionUserId(null);
      }

    }
    fetchUserId();}
    catch{
      setSessionUserId(null);
    }
  }, []);

  return (
    <div className="p-5">
      <div className="text-left text-cyan-900 font-semibold underline">{props.username}</div>
      {isEditing ? (
      //* display the editing form if editing is clicked *//
      <div>
        <form className="p-8 text-xl" onSubmit={handleSubmitMessage}>
          <textarea
            name="updatedcontent"
            value={props.updateContent}
            onInput={props.updateMessageContent}
            onClick={event => event.stopPropagation()}
            className="p-2 border-2 border-gray-500 w-2/3 text-center"
          /> 
          <div>
            <button>Send</button>
          </div>
        </form>
      </div>
      ):( 
      <p className="border-b-1 p-5">{props.content}</p>
      )}
      <div className="inline-flex items-center space-x-5 p-2">
        <div className='inline-flex space-x-1 items-center'>
          <FavoriteBorderIcon />
          <p className="text-xs">{props.upvotes}</p>
          <p className="text-xs">{props.postdate.toLocaleDateString()}</p>
        </div>
      {sessionUserId == props.userid? (
        /* display the edit icon if the user is the owner of the message */
        <EditIcon className="cursor-pointer" onClick={handleEditClick}/>
      ):( 
        null
      )}
      </div>
   </div>
  );
}
export default TopicMessage;