import ChatBubbleOutlineIcon from '@mui/icons-material/ChatBubbleOutline';
import EditIcon from '@mui/icons-material/Edit';
import DeleteForeverTwoToneIcon from '@mui/icons-material/DeleteForeverTwoTone';
import { useEffect, useState } from 'react';
import { GetUserRole } from '../functions/GetUserRole';

type TopicProps = {
  topicid: number;
  topicname: string;
  messagecount: number;
  lastupdated: Date;
  userid: string;
  addTopicName: (event: React.ChangeEvent<HTMLInputElement>) => void;
  sendTopicName: (event: React.FormEvent<HTMLFormElement>, topicid:number) => void;
  deleteTopic: (event: React.MouseEvent, topicid: number) => void;
}

/**
 * Topic component for the discussion forum
 * @param props Props for the topic
 * @returns Topic component
 */

function Topic(props: TopicProps) {

  const [userrole, setUserRole] = useState<string | null>(null);
  const [isEditing, setIsEditing] = useState(false);

  /**
   * Stop further propagations of the event and toggle the edit mode before editing the topic name
   * @param event MouseEvent
   */
  const handleEditClick = (event: React.MouseEvent) => {
    event.stopPropagation();
    setIsEditing(!isEditing);
  };

  /**
   * submit the updated name of the topic
   * @param event FormEvent
   */
  const handleSubmitForm = (event: React.FormEvent<HTMLFormElement>) => {
    props.sendTopicName(event, props.topicid);
    setIsEditing(false);
  };

  /**
   * delete a topic
   * @param event MouseEvent
   */
  const handleDeleteClick = (event: React.MouseEvent) => {
    event.stopPropagation();
    props.deleteTopic(event, props.topicid);
  };

    /**
     * Get the user id from the server
     */
    useEffect(() => {
      async function fetchUserRole() {
        const role = await GetUserRole();
        if (role) {
          setUserRole(role);
        } else {
          console.error('Failed to fetch user ID');
        }
  
      }
      fetchUserRole();
    }, []);

  return (
    <div className='border-2 w-3xl border-b-cyan-900'>
      <div className='p-1'>
      {userrole == "Admin" ? (
      <DeleteForeverTwoToneIcon onClick={handleDeleteClick}/>):(null)}
      </div>
      {isEditing ? (
        //* display the editing form if editing is clicked *//
        <div className='border-b-1 border-b-neutral-400 inline-flex w-full justify-center items-center space-x-3 p-5'>
        <form onSubmit={handleSubmitForm}>
          <input
            name="updateTopicName"
            type="text"
            placeholder="Edit topic name"
            onInput={props.addTopicName}
            onClick={event => event.stopPropagation()}
            className="text-amber-800 text-3xl"/>
          <EditIcon onClick={handleEditClick}/> 
        </form> 
        </div>
      //* If no messages and not editing, display the topic name with edit icon *//
      ) : props.messagecount == 0 && !isEditing && userrole=="Admin"? (
      <div className='border-b-1 border-b-neutral-400 inline-flex w-full justify-center items-center space-x-3 p-5'>
        <h1 className="uppercase text-amber-800">{props.topicname}</h1>
        <EditIcon onClick={handleEditClick}/>
      </div>
      //* if messages are posted on the topic, only display the topic name *//
       ) :
      <div className='border-b-1 p-5'>
        <h1 className="uppercase text-amber-800">{props.topicname}</h1>
      </div>
      }
      <div className='inline-flex space-x-5 items-center p-2'>
        <p className='inline-flex items-center'>
          {props.messagecount}
          <ChatBubbleOutlineIcon className='m-1'/>
        </p>
        {props.messagecount > 0 ? (
        <p>Latest message: {props.lastupdated.toLocaleDateString()}</p>) : 
        <p>No messages posted!</p>}
      </div>
    </div>
  );
}
export default Topic;