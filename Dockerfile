FROM mcr.microsoft.com/dotnet/aspnet:5.0

ENV ASPNETCORE_ENVIRONMENT=Production

RUN apt-get update && apt-get install -y libgdiplus fontconfig fontconfig-config fonts-dejavu-core libbsd0 \
                                                        libexpat1 libfontconfig1 libfontenc1 libfreetype6 libjpeg62-turbo libpng16-16 libx11-6 libx11-data libxau6 libxcb1 \
                                                        libxdmcp6 libxext6 libxrender1 lsb-base sensible-utils ucf x11-common xfonts-75dpi xfonts-base xfonts-encodings \
                                                        xfonts-utils cabextract libmspack0 wget

RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /etc/ssl/openssl.cnf
RUN sed -i 's/DEFAULT@SECLEVEL=2/DEFAULT@SECLEVEL=1/g' /usr/lib/ssl/openssl.cnf
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/g' /usr/lib/ssl/openssl.cnf

WORKDIR /app

COPY ./Release ./

ADD http://deb.debian.org/debian/pool/contrib/m/msttcorefonts/ttf-mscorefonts-installer_3.7_all.deb /ttf-mscorefonts-installer.deb
RUN dpkg -i /ttf-mscorefonts-installer.deb
RUN fc-cache -f
RUN rm /ttf-mscorefonts-installer.deb

EXPOSE 80

CMD ["dotnet", "CHESF.COMPRAS.API.dll"]
