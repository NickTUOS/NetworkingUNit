import SocketServer
import sys
import logging

logging.basicConfig(level=logging.DEBUG, format = '%(name)s: %(message)s',)

class RequestHandler (SocketServer.BaseRequestHandler):
    def __init__ (self, request, clientAddress, server):
        self.logger = logging.getLogger('RequestHandeler')
        self.logger.debug('__init__')
        SocketServer.BaseRequestHandler.__init__(self, request, clientAddress, server)
        return

    def setup(self):
        self.logger.debug('setup')
        return SocketServer.BaseRequestHandler.setup(self)

    def handle(self):
        self.logger.debug('handle')
        
        #Echo message back to client
        data = self.request.recv(1024)
        self.logger.debug('recv()->"%s"', data)
        self.request.send(data)
        return

    def finish(self):
        self.logger.debug('finish')
        return  SocketServer.BaseRequestHandler.finish(self)





class EchoServer(SocketServer.ThreadingMixIn, SocketServer.TCPServer):
    def __init__(self, server_address, handler_class=RequestHandler):
        self.logger = logging.getLogger('EchoServer')
        self.logger.debug('__init__')
        SocketServer.TCPServer.__init__(self, server_address, handler_class)
        self.Run = True
        return

    def server_activate(self):
        self.logger.debug('server_activate')
        SocketServer.TCPServer.server_activate(self)
        return

    def serve_forever(self):
        self.logger.debug('waiting for request')
        self.logger.info('Handling requests, press <Ctrl-C> to quit')
        while self.Run:
            self.handle_request()
        return

    def handle_request(self):
        self.logger.debug('handle_request')
        return SocketServer.TCPServer.handle_request(self)

    def verify_request(self, request, client_address):
        self.logger.debug('verify_request(%s, %s)', request, client_address)
        return SocketServer.TCPServer.verify_request(self, request, client_address)

    def process_request(self, request, client_address):
        self.logger.debug('process_request(%s, %s)', request, client_address)
        return SocketServer.TCPServer.process_request(self, request, client_address)

    def server_close(self):
        self.logger.debug('server_close')
        return SocketServer.TCPServer.server_close(self)

    def finish_request(self, request, client_address):
        self.logger.debug('finish_request(%s, %s)', request, client_address)
        return SocketServer.TCPServer.finish_request(self, request, client_address)

    def close_request(self, request_address):
        self.logger.debug('close_request(%s)', request_address)
        return SocketServer.TCPServer.close_request(self, request_address)



if __name__ == '__main__':
    import socket
    import threading

    Address = ('localhost', 1234)
    Server = EchoServer(Address, RequestHandler)
    Ip, Port = Server.server_address # find what ip address and port we are using
    
    try:
        Server.serve_forever()
    except KeyboardInterrupt:
        pass

    #ServerThread = threading.Thread(target=Server.serve_forever)
    #ServerThread.setDaemon(True) # dont hang on exit
    #ServerThread.start()
    
    ##client
    #logger = logging.getLogger('client')
    #logger.info('Server on %s %s', Ip, Port)
    
    ##connect to server
    #logger.debug('create socket')
    #Socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    #logger.debug('connecting to sesrver')
    #Socket.connect((Ip,Port))

    ##send data
    #Message = 'Hello world!'
    #logger.debug('sending message %s', Message)
    #MessageBytes = Message.encode()
    #len_sent = Socket.send(MessageBytes)

    ##recive data 
    #logger.debug('waiting for responce')
    #responce = Socket.recv(len_sent)
    #logger.debug('responce from server: %s', responce)

    #clean up
    
    Server.socket.close()
    logger.debug('server.socket closed')
    Server.shutdown()
    logger.debug('server shutdown()')



    