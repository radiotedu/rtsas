using IcecastStreamMonitor.Services;
using Xunit;

namespace IcecastStreamMonitor.Tests;

public class TinyIceImporterTests
{
    [Fact]
    public void ImportFromTinyIceJson_ParsesAllMountsAndPort_Successfully()
    {
        var sampleJson = """
        {
            "bind_host": "0.0.0.0",
            "port": "11154",
            "default_source_password": "$2a$12$EKOae0wsbOfqTfKZLzWjRuQK0s2z1btLXTNNPX5Q9VnkwSaTVX/kS",
            "mounts": {
                "/ai": "$2a$12$AFr5XPCSpYLo6opooGFbq.EMH8SQT2wabtXXvpJsAkwZgRlFRSmm",
                "/cazz": "$2a$12$tYbposMbQdhR5hqfk1in7eGmJE6eqaA3KK57OBW99RypEtGPFSXAi",
                "/cazz-flac": "$2a$12$Cjqlq5HYLaxh38iKNvMgd.o13J6/FK0m0yMfn2cc3q2UXe7DKNPuy",
                "/cazz-high": "$2a$12$X/GA7uCQoYAwfYDGdYsVk.T4rNThFr4t6slGfTwLc67xduq/BmPbC",
                "/cazz-low": "$2a$12$PR/bw.TaOIxYCav9WIsxuOsB9gJjkrDEDNH8uhGZlxf456CsUoLqG",
                "/cazz-normal": "$2a$12$ypu/ygePLVY0lH4ZjPxYVu8u.wlVpyKd4ncWHq1fHpRrrbJrkBUv2",
                "/classic": "$2a$12$B2hQXu19AlmoHlx1M9s/p.kd9szPs/4FsN4aXRu0sYxzinqmk5N9C",
                "/classic-flac": "$2a$12$lsAU9g9mqgVL7XgYxCUL6OeUXWyXpGmBxmAWSz6tx1kefqGyTGSb2",
                "/classic-high": "$2a$12$1Y6pyV1U1TJDZLiiJQVveusxoLekEh7achciZlnYs04oZtpiXBZiu",
                "/classic-low": "$2a$12$F6rHDSHDm5Ln4JaSVzmdB.STctZ0bTlQ2wlW5KjP1YCf8XkC1A95C",
                "/classic-normal": "$2a$12$7dten8QZcoavbQBTFl20Xe.Tpt7YTUZlxUm35Kj/ssCRpHebJXin2",
                "/en": "$2a$12$2BLK9nCzoWmtP58gIAZYUucvEGp6rFh04Xkc97phDkudlKTHjbdYK",
                "/energize": "$2a$12$iNBhbRiq0m/CfnTtGg1qvu9vGJtiBgMMd8nFqXAs48JfkOB5WSyNy",
                "/energize-flac": "$2a$12$qD.ry7a1Ye4O6iKnPmdaYOZMYIZ5Tpf3r/lqkR2AJfCy0jAOnye4W",
                "/energize-high": "$2a$12$iVIyALmbCOQabTIiAVpC9uLbz.vk0afGE.08.NDqhcahGiGqCxft.",
                "/energize-low": "$2a$12$H.yL91xJp0oQrWc2.y/8ZeepianSdZLX84KBvIACH7YQgsm9XvO36",
                "/energize-normal": "$2a$12$LYnqSWLH8uaf.NXNd6nqWuJVJkdhLXPaHA.Hkvr.TKr2rikyNVdAy",
                "/event": "$2a$12$csE.hLmxYVxbPwkgwzt9beZFUKuaJ8qrTwGTLQmaithUt2vvqtNM.",
                "/fr": "$2a$12$hg.65YdXw8KSQrrHQA59lev/cdzQ7aVzZKzweuvV6j90Fi9pQ88Iy",
                "/lofi": "$2a$12$5g9.kVIH/OFJAnOFjD/sL.yw3ExxM4VQL7X8mV/T0V3jDKcLh5hIC",
                "/lofi-flac": "$2a$12$RdJYJjae5xmcmnyqKbMgHOrkV5p7uNPlXeipPz1XWTkkrQoYhs212",
                "/lofi-high": "$2a$12$wsuBOYXDsoINlzogIlTf6etlaKmVRhbu0Jg7v9zPhGrLBvG8RbgSq",
                "/lofi-low": "$2a$12$veMKbuLDVWcs/11ivywA7ewQbH4Ki2LrujWi9gdSYdvlBmRpl/sQC",
                "/lofi-normal": "$2a$12$c1eR6YuQC9NjWsarLjx/aOw.H0aLfKZuUrhgex4uui6F8sQF5MUBC",
                "/radio": "$2a$12$wB.PkiNgiXAra9NkpaATXe/rmk1gNnKU7RzETfWcwpLXUY4MpOok.",
                "/radio-flac": "$2a$12$Rp/E9fJfOX0Fb2syNi10MufKbzFy/30E3.JZ7lIqqCyUzr6YdvRFS",
                "/radio-high": "$2a$12$0.aclY4Oj8KCctgiKJIA/.ijQht8liS7N2wucTHLILnouKDLhkqtG",
                "/radio-low": "$2a$12$eUJqeMfEAFvUBzbq3mNb6O2z8aPBNaq3DfEN9kar718GfLaEt.39G",
                "/radio-normal": "$2a$12$nWvF57HUBTkQxY4wrQ0B8Ona4ruUZB09Fj7VDG/xez50fh2ue6wra",
                "/rock": "$2a$12$YCsNDjQ6HFZ..EGXC25AmumfzvMhNj2QpFhZ5l0PETrZmivNo4gGi",
                "/rock-flac": "$2a$12$jJQqVOa.G9XJMO6H.OJPVeOLC/lD1kJTr6624fulM7K7277pGA.5S",
                "/rock-high": "$2a$12$T5RvFYi9/dK7eYJvSvYCdeYtZOcO0qxptBszlovvxma/qmNIZrhwe",
                "/rock-low": "$2a$12$7c9vxirm7issJMKU0t659.R.al2FqrJFlcFz3mh8rrWRmvCqaBawi",
                "/rock-normal": "$2a$12$zKZx1yC1u9/XQl8jdkEvU.ACiMWQW5RwoZv1rtHz2.rq5ez2JPok2",
                "/spark": "$2a$12$PK5of6LuqdwDLv/KykvdSuGKbP0ULNLiCWPbsfMjrFGPv9E/FBwLG"
            },
            "trusted_proxies": [
                "127.0.0.1"
            ],
            "admin_user": "radiotedu",
            "hostname": "stream.radiotedu.com"
        }
        """;

        var result = TinyIceConfigImporter.ImportFromTinyIceJson(sampleJson);

        Assert.Equal(11154, result.Port);
        Assert.Equal("stream.radiotedu.com", result.Hostname);
        Assert.Equal("radiotedu", result.AdminUser);
        Assert.Equal(35, result.TotalMountsCount);
        Assert.Contains("127.0.0.1", result.TrustedProxies);

        var lofiMount = result.Channels.Find(c => c.MountPath == "/lofi");
        Assert.NotNull(lofiMount);
        Assert.Equal(11154, lofiMount.Port);
        Assert.True(lofiMount.Enabled);

        var cazzFlacMount = result.Channels.Find(c => c.MountPath == "/cazz-flac");
        Assert.NotNull(cazzFlacMount);
        Assert.Contains("(FLAC)", cazzFlacMount.Name);
    }
}
