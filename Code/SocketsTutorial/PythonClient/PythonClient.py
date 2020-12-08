if __name__ == '__main__':
    import socket
    import sys
    import logging
    
    logging.basicConfig(level=logging.DEBUG)
    
    Ip = '127.0.0.1'
    Port = 1234 
    Address = (Ip, Port)

    #client
    logger = logging.getLogger('client')
    logger.info('Server on %s %s', Ip, Port)
    
    #connect to server
    logger.debug('create socket')
    Socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    logger.debug('connecting to sesrver')
    Socket.connect((Ip,Port))

    #send data
    Message = raw_input("type a message and have it returned: ")
    #Message+= '<EOF>'
    logger.debug('sending message %s', Message)
    MessageBytes = Message.encode()
    len_sent = Socket.send(MessageBytes)

    #recive data 
    logger.debug('waiting for responce')
    responce = Socket.recv(len_sent)
    logger.debug('responce from server: %s', responce)

    logger.debug('Closing socket')
    Socket.close()
    logger.debug('done')
    print('done')
