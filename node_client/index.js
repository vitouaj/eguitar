const amqplib = require("amqplib");

const listen = async () => {
  const queue = "user-created-event";
  const conn = await amqplib.connect("amqp://localhost:5672");

  const ch1 = await conn.createChannel();
  await ch1.assertQueue(queue);

  // Listener
  ch1.consume(queue, (msg) => {
    if (msg !== null) {
      console.log("Recieved:", msg.content.toString());
      ch1.ack(msg);
    } else {
      console.log("Consumer cancelled by server");
    }
  });
};

console.log("node consumer listening...");
listen();
