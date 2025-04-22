import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import EditIcon from '@mui/icons-material/Edit';
import React, { useState } from 'react';

type MessageProps = {
  content: string;
  upvotes: number;
  postdate: Date;
  userid: number;
  messageid: number;
  updateContent:string;
  sendUpdatedMessage: (event: React.FormEvent<HTMLFormElement>, messageid: number) => void;
  updateMessageContent: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
  setInitialContent: React.Dispatch<React.SetStateAction<{updatedcontent: string}>>;
};

function TopicMessage (props: MessageProps) {

  const sessionuserid = sessionStorage.getItem('id'); 
  const [isEditing, setIsEditing] = useState(false);

  const handleEditClick = (event: React.MouseEvent) => {
    event.stopPropagation();
    props.setInitialContent({updatedcontent: props.content});
    setIsEditing(!isEditing);
  }

  const handleSubmitMessage = (event: React.FormEvent<HTMLFormElement>) => {
    console.log("checkpoint 0");
    props.sendUpdatedMessage(event, props.messageid);
    setIsEditing(false);
  };

  return (
    <div className="border-2 border-b-neutral-950 p-1 w-2xl text-xs m-2">
      {isEditing ? (
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
        {sessionuserid == props.userid.toString()? (
          <EditIcon className="cursor-pointer" onClick={handleEditClick}/>
        ) : ( null )}
        </div>
    </div>
  );
}
export default TopicMessage;