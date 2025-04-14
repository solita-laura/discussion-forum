import ChatBubbleOutlineIcon from '@mui/icons-material/ChatBubbleOutline';

type TopicProps = {
  topicname: string;
  messagecount: number;
  lastupdated: Date;
};

function Topic(props: TopicProps) {
  return (
    <div className="border-2 border-b-neutral-950 p-1 w-2xl text-xs">
      <h1 className="p-5 text-xs text-orange-800 border-b-1 border-b-gray-400">{props.topicname}</h1>
        <div className='inline-flex items-center space-x-1 p-2'>
            <p className="text-xs">{props.messagecount}</p>
            <ChatBubbleOutlineIcon/>
        </div>
        <div></div>
        <p>Latest message: {props.lastupdated.toLocaleDateString()}</p>
    </div>
  );
}
export default Topic;