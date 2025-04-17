import ChatBubbleOutlineIcon from '@mui/icons-material/ChatBubbleOutline';
import EditIcon from '@mui/icons-material/Edit';
import { useState } from 'react';

type TopicProps = {
  topicid: number;
  topicname: string;
  messagecount: number;
  lastupdated: Date;
  addTopicName: (event: React.ChangeEvent<HTMLInputElement>) => void;
  sendTopicName: (event: React.FormEvent<HTMLFormElement>, topicid:number) => void;
}

function Topic(props: TopicProps) {

  const [isEditing, setIsEditing] = useState(false);

  const handleEditClick = (event: React.MouseEvent) => {
    event.stopPropagation();
    setIsEditing(!isEditing);
  };

  const handleSubmitForm = (event: React.FormEvent<HTMLFormElement>) => {
    props.sendTopicName(event, props.topicid);
    setIsEditing(false);
  };

  return (
    <div className="border-2 border-b-neutral-950 p-1 w-2xl text-xs">
      {props.messagecount == 0 && isEditing ? (
        <form className="p-8 text-xl" onSubmit={handleSubmitForm}>
          <input
            name="topicname"
            type="text"
            placeholder="Edit topic name"
            onInput={props.addTopicName}
            onClick={event => event.stopPropagation()}
            className="p-5 text-orange-800 border-b-1 border-b-gray-400"
          />
          <EditIcon onClick={handleEditClick} className='m-2'/> 
        </form> 
      ) : props.messagecount == 0 && !isEditing ? (
      <h1 className="p-5 text-xs text-orange-800 border-b-1 border-b-gray-400">
        {props.topicname}
        <EditIcon onClick={handleEditClick} className="cursor-pointer" />
      </h1>
       ) :
      <h1 className="p-5 text-xs text-orange-800 border-b-1 border-b-gray-400">{props.topicname}</h1>
       }
        <div className='inline-flex items-center space-x-1 p-2'>
            <p className="text-xs">{props.messagecount}</p>
            <ChatBubbleOutlineIcon/>
        </div>
        <div></div>
        {props.messagecount > 0 ? (
        <p>Latest message: {props.lastupdated.toLocaleDateString()}</p>) : 
        <p>No messages posted!</p>}
    </div>
  );
}
export default Topic;