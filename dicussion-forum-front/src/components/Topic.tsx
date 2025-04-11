import ChatBubbleOutlineIcon from '@mui/icons-material/ChatBubbleOutline';

type TopicProps = {
  topicname: string;
  messagecount: number;
  lastupdated: Date;
};

function Topic(props: TopicProps) {
  return (
    <div className="border-2 border-cyan-950 p-2 w-2xl text-xs">
      <h1 className="text-xs text-orange-800">{props.topicname}</h1>
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